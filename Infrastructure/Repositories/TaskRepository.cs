using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Domain.Entities.Task task, CancellationToken cancellationToken = default)
        {
            _context.Add(task);
            return _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Domain.Entities.Task?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks.FindAsync([id], cancellationToken);
        }

        public async Task<List<Domain.Entities.Task?>> GetByColumnIdAsync(Guid columnId, CancellationToken cancellationToken = default)
        {
            return _context.Tasks.Where(t => t.ColumnId == columnId).ToList();
        }

        public Task RemoveAsync(Domain.Entities.Task task, CancellationToken cancellationToken = default)
        {
            _context.Tasks.Remove(task);
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
