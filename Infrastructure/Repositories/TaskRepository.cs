using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class TaskRepository(ApplicationDbContext context) : ITaskRepository
    {
        public Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        public Task<List<TaskItem>> GetByColumnAsync(Guid columnId, CancellationToken cancellationToken = default) =>
            context.Tasks
                .AsNoTracking()
                .Where(t => t.ColumnId == columnId)
                .OrderBy(t => t.Order)
                .ToListAsync(cancellationToken);
    }
}
