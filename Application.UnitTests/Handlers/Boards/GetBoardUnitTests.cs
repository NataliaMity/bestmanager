using Application.Common;
using Application.Handlers.Boards.GetBoardById;
using Application.Handlers.Boards.GetBoards;
using Application.UnitTests.Fakes;

namespace Application.UnitTests.Handlers.Boards
{
    public class GetBoardUnitTests
    {
        private readonly InMemoryStore _store = new();

        [Fact]
        public async Task GetBoards_NoBoards_ReturnsEmptyList()
        {
            var boards = await new GetBoardsHandler(_store.BoardRepository).Handle();

            Assert.Empty(boards);
        }

        [Fact]
        public async Task GetBoards_ReturnsAllBoards()
        {
            _store.AddBoard("A");
            _store.AddBoard("B");

            var boards = await new GetBoardsHandler(_store.BoardRepository).Handle();

            Assert.Equal(new[] { "A", "B" }, boards.Select(b => b.Name));
        }

        [Fact]
        public async Task GetBoardById_ReturnsColumnsAndTasksInOrder()
        {
            var board = _store.AddBoard();
            var todo = _store.AddColumn(board, "To do");
            var done = _store.AddColumn(board, "Done");
            todo.AddTask("Первая", null);
            todo.AddTask("Вторая", null);

            var handler = new GetBoardByIdHandler(_store.BoardRepository, _store.ColumnRepository);
            var result = await handler.Handle(new GetBoardByIdHandler.GetBoardByIdQuery(board.Id));

            Assert.Equal(board.Id, result.Id);
            Assert.Equal(new[] { "To do", "Done" }, result.Columns.Select(c => c.Name));
            Assert.Equal(new[] { "Первая", "Вторая" }, result.Columns[0].Tasks.Select(t => t.Name));
            Assert.Empty(result.Columns[1].Tasks);
        }

        [Fact]
        public async Task GetBoardById_UnknownBoard_ThrowsNotFound()
        {
            var handler = new GetBoardByIdHandler(_store.BoardRepository, _store.ColumnRepository);

            await Assert.ThrowsAsync<NotFoundException>(
                () => handler.Handle(new GetBoardByIdHandler.GetBoardByIdQuery(Guid.NewGuid())));
        }
    }
}
