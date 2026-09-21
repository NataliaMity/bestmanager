using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class BoardConfiguration : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            builder.ToTable("Boards");

            builder.HasKey(b => b.Id);
            // Id генерирует домен. Без этого EF примет новую сущность за существующую.
            builder.Property(b => b.Id).ValueGeneratedNever();

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(Board.NameMaxLength);
            builder.Property(b => b.Description)
                .IsRequired()
                .HasMaxLength(Board.DescriptionMaxLength);
            builder.Property(b => b.Created)
                .IsRequired();
            builder.Property(b => b.Updated)
                .IsRequired();
        }
    }
}
