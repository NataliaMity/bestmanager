using Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// Репозитории только отслеживают изменения. Сохраняет их <see cref="IUnitOfWork"/>.
    /// </summary>
    public interface IBoardRepository
    {
        Task<Board?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Board>> GetAllAsync(CancellationToken cancellationToken = default);

        void Add(Board board);
        void Remove(Board board);
    }
}
