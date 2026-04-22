using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IColumnRepository
    {
        Task<Column?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Column>?> GetByBoardAsync(Guid boardId, CancellationToken cancellationToken = default);

        System.Threading.Tasks.Task AddAsync(Column column, CancellationToken cancellationToken = default);
        System.Threading.Tasks.Task RemoveAsync(Column column, CancellationToken cancellationToken = default);
    }
}
