using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class ColumnRepository : IColumnRepository
    {
        private readonly ApplicationDbContext _context;

        public ColumnRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Domain.Entities.Column column, CancellationToken cancellationToken = default)
        {
            _context.Columns.Add(column);
            return _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Domain.Entities.Column?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Columns.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<List<Domain.Entities.Column>?> GetByBoardAsync(Guid boardId, CancellationToken cancellationToken = default)
        {
            return _context.Columns.Where(c => c.BoardId == boardId).ToList();
        }

        public Task RemoveAsync(Domain.Entities.Column column, CancellationToken cancellationToken = default)
        {
            _context.Columns.Remove(column);
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}