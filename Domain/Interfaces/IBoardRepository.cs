using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBoardRepository
    {
        Task<Board?> GetByIDAsync(Guid id, CancellationToken cancellationToken = default);

        Board AddAsync(Board board, CancellationToken cancellationToken = default);
        Board DeleteAsync(Board board, CancellationToken cancellationToken = default);
        Board UpdateAsync(Board board, CancellationToken cancellationToken = default);
    }
}
