using Domain.Interfaces;

namespace Infrastructure.Data
{
    internal class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            context.SaveChangesAsync(cancellationToken);
    }
}
