using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<Entities.Task?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Entities.Task>?> GetByColumnIdAsync(Guid columnId, CancellationToken cancellationToken = default);
        Task<List<Entities.Task>?> GetByBoardIdAsync(Guid boardId, CancellationToken cancellationToken = default);

        System.Threading.Tasks.Task AddAsync(Entities.Task task, CancellationToken cancellationToken = default);
        System.Threading.Tasks.Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        System.Threading.Tasks.Task UpdateAsync(Entities.Task task, CancellationToken cancellationToken = default);
    }
}
