using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = Domain.Entities.Task;

namespace Infrastructure.Data.Configuration
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(t => t.Description)
                .HasMaxLength(500);
            builder.Property(t => t.Created)
                .IsRequired();
            builder.Property(t => t.Updated)
                .IsRequired();

            builder.HasOne(t => t.Column)
            .WithMany()
            .HasForeignKey(t => t.ColumnId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}