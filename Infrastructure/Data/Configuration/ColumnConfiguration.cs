using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class ColumnConfiguration : IEntityTypeConfiguration<Column>
    {
        public void Configure(EntityTypeBuilder<Column> builder)
        {
            builder.ToTable("Columns");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedNever();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(Column.NameMaxLength);
            builder.Property(c => c.Order)
                .IsRequired();
            builder.Property(c => c.Created)
                .IsRequired();
            builder.Property(c => c.Updated)
                .IsRequired();

            // Колонка ссылается на доску только по Id — это разные агрегаты
            builder.HasOne<Board>()
                .WithMany()
                .HasForeignKey(c => c.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Tasks)
                .WithOne()
                .HasForeignKey(t => t.ColumnId)
                .OnDelete(DeleteBehavior.Cascade);

            // Коллекция задач доступна снаружи только на чтение, EF пишет в приватное поле _tasks
            builder.Navigation(c => c.Tasks)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(c => new { c.BoardId, c.Order });
        }
    }
}
