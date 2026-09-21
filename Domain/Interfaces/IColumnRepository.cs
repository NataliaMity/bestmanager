using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IColumnRepository
    {
        /// <summary>Колонка вместе с задачами (весь агрегат).</summary>
        Task<Column?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>Колонка, в которой лежит задача, вместе со всеми её задачами.</summary>
        Task<Column?> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default);

        /// <summary>Колонки доски, отсортированные по порядку.</summary>
        Task<List<Column>> GetByBoardAsync(Guid boardId, bool includeTasks = false, CancellationToken cancellationToken = default);

        void Add(Column column);
        void Remove(Column column);
    }
}
