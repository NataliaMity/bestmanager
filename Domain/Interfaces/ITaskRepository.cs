
namespace Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<Entities.Task?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Entities.Task>?> GetByColumnIdAsync(Guid columnId, CancellationToken cancellationToken = default);

        Task AddAsync(Entities.Task task, CancellationToken cancellationToken = default);
        Task RemoveAsync(Entities.Task task, CancellationToken cancellationToken = default);
    }
}
