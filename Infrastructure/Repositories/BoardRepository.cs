using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class BoardRepository(ApplicationDbContext context) : IBoardRepository
    {
        public Task<Board?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Boards.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        public Task<List<Board>> GetAllAsync(CancellationToken cancellationToken = default) =>
            context.Boards
                .AsNoTracking()
                .OrderBy(b => b.Created)
                .ToListAsync(cancellationToken);

        public void Add(Board board) => context.Boards.Add(board);

        // Колонки и задачи удалятся каскадно в БД
        public void Remove(Board board) => context.Boards.Remove(board);
    }
}
