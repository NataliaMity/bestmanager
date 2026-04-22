using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly ApplicationDbContext _context;

        public BoardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Board?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Boards.FindAsync(new object[] { id }, cancellationToken);
        }

        public Task AddAsync(Domain.Entities.Board board, CancellationToken cancellationToken = default)
        {
            _context.Boards.Add(board);
            return _context.SaveChangesAsync(cancellationToken);
        }

        public Task RemoveAsync(Domain.Entities.Board board, CancellationToken cancellationToken = default)
        {
            _context.Boards.Remove(board);
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
