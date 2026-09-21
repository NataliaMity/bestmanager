using Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// Только чтение. Задачи изменяются через агрегат <see cref="Column"/> (<see cref="IColumnRepository"/>).
    /// </summary>
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>Задачи колонки, отсортированные по порядку.</summary>
        Task<List<TaskItem>> GetByColumnAsync(Guid columnId, CancellationToken cancellationToken = default);
    }
}
