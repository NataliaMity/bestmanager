using Domain.Entities;
using Domain.Interfaces;

namespace Application.UnitTests.Fakes
{
    /// <summary>
    /// Простые in-memory реализации репозиториев. Проще моков: тест проверяет состояние,
    /// а не то, какие методы вызывались.
    /// </summary>
    public class InMemoryStore
    {
        public List<Board> Boards { get; } = new();
        public List<Column> Columns { get; } = new();
        public FakeUnitOfWork UnitOfWork { get; } = new();

        public FakeBoardRepository BoardRepository => new(this);
        public FakeColumnRepository ColumnRepository => new(this);
        public FakeTaskRepository TaskRepository => new(this);

        public Board AddBoard(string name = "Доска")
        {
            var board = new Board(name, null);
            Boards.Add(board);
            return board;
        }

        public Column AddColumn(Board board, string name = "Колонка")
        {
            var column = new Column(board.Id, name, Columns.Count(c => c.BoardId == board.Id));
            Columns.Add(column);
            return column;
        }
    }

    public class FakeUnitOfWork : IUnitOfWork
    {
        public int SaveCount { get; private set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SaveCount++;
            return Task.FromResult(0);
        }
    }

    public class FakeBoardRepository(InMemoryStore store) : IBoardRepository
    {
        public Task<Board?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromResult(store.Boards.FirstOrDefault(b => b.Id == id));

        public Task<List<Board>> GetAllAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(store.Boards.ToList());

        public void Add(Board board) => store.Boards.Add(board);

        public void Remove(Board board)
        {
            store.Boards.Remove(board);
            store.Columns.RemoveAll(c => c.BoardId == board.Id);
        }
    }

    public class FakeColumnRepository(InMemoryStore store) : IColumnRepository
    {
        public Task<Column?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromResult(store.Columns.FirstOrDefault(c => c.Id == id));

        public Task<Column?> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default) =>
            Task.FromResult(store.Columns.FirstOrDefault(c => c.Tasks.Any(t => t.Id == taskId)));

        public Task<List<Column>> GetByBoardAsync(Guid boardId, bool includeTasks = false, CancellationToken cancellationToken = default) =>
            Task.FromResult(store.Columns.Where(c => c.BoardId == boardId).OrderBy(c => c.Order).ToList());

        public void Add(Column column) => store.Columns.Add(column);

        public void Remove(Column column) => store.Columns.Remove(column);
    }

    public class FakeTaskRepository(InMemoryStore store) : ITaskRepository
    {
        public Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromResult(store.Columns.SelectMany(c => c.Tasks).FirstOrDefault(t => t.Id == id));

        public Task<List<TaskItem>> GetByColumnAsync(Guid columnId, CancellationToken cancellationToken = default) =>
            Task.FromResult(store.Columns
                .Where(c => c.Id == columnId)
                .SelectMany(c => c.Tasks)
                .OrderBy(t => t.Order)
                .ToList());
    }
}
