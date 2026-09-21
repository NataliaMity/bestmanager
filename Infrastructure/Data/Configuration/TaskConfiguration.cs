using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class TaskConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedNever();

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(TaskItem.NameMaxLength);
            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(TaskItem.DescriptionMaxLength);
            builder.Property(t => t.Order)
                .IsRequired();
            builder.Property(t => t.Created)
                .IsRequired();
            builder.Property(t => t.Updated)
                .IsRequired();

            // Связь с колонкой настроена в ColumnConfiguration

            builder.HasIndex(t => new { t.ColumnId, t.Order });
        }
    }
}
