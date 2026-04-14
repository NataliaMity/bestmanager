using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IColumnRepository
    {
        Task<Column?> GetByIDAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Column>?> GetByBoardAsync(Board board, CancellationToken cancellationToken = default);

        Column AddAsync(Column column, CancellationToken cancellationToken = default);
        Column DeleteAsync(Column column, CancellationToken cancellationToken = default);
        Column UpdateAsync(Column column, CancellationToken cancellationToken = default);
    }
}
