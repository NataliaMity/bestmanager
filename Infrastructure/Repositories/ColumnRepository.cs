using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class ColumnRepository(ApplicationDbContext context) : IColumnRepository
    {
        public Task<Column?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            WithTasks().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public Task<Column?> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default) =>
            WithTasks().FirstOrDefaultAsync(c => c.Tasks.Any(t => t.Id == taskId), cancellationToken);

        public Task<List<Column>> GetByBoardAsync(Guid boardId, bool includeTasks = false, CancellationToken cancellationToken = default)
        {
            var query = includeTasks ? WithTasks() : context.Columns;
            return query
                .Where(c => c.BoardId == boardId)
                .OrderBy(c => c.Order)
                .ToListAsync(cancellationToken);
        }

        public void Add(Column column) => context.Columns.Add(column);

        public void Remove(Column column) => context.Columns.Remove(column);

        private IQueryable<Column> WithTasks() =>
            context.Columns.Include(c => c.Tasks.OrderBy(t => t.Order));
    }
}
