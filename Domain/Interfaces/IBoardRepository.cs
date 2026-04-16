using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBoardRepository
    {
        Task<Board?> GetByIDAsync(Guid id, CancellationToken cancellationToken = default);

        System.Threading.Tasks.Task AddAsync(Board board, CancellationToken cancellationToken = default);
        System.Threading.Tasks.Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        System.Threading.Tasks.Task UpdateAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
