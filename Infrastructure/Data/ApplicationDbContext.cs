using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Task = Domain.Entities.Task;


namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Column> Columns => Set<Column>();
        public DbSet<Task> Tasks => Set<Task>();
        public DbSet<Board> Boards => Set<Board>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}