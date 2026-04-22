using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    public class BoardConfiguration : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(b => b.Created)
                .IsRequired();
            builder.Property(b => b.Updated)
                .IsRequired();
        }
    }
}
