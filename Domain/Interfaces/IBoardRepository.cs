using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Domain.Interfaces
{
    public interface IBoardRepository
    {
        Task<Board?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task AddAsync(Board board, CancellationToken cancellationToken = default);
        Task RemoveAsync(Board board, CancellationToken cancellationToken = default);
    }
}
