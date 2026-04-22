using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class ColumnConfiguration : IEntityTypeConfiguration<Column>
    {
        public void Configure(EntityTypeBuilder<Column> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.BoardId)
                .IsRequired();
            builder.Property(c => c.Created)
                .IsRequired();
            builder.Property(c => c.Updated)
                .IsRequired();

            builder.HasOne(c => c.Board)
            .WithMany()
            .HasForeignKey(c => c.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
