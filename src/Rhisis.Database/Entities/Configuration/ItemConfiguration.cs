using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rhisis.Database.Entities.Configuration
{
    internal class ItemConfiguration : IEntityTypeConfiguration<DbItem>
    {
        public void Configure(EntityTypeBuilder<DbItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.GameItemId).IsRequired();
            builder.Property(x => x.Refine).HasColumnType("TINYINT").IsRequired(false);
            builder.Property(x => x.Element).HasColumnType("TINYINT").IsRequired(false);
            builder.Property(x => x.ElementRefine).HasColumnType("TINYINT").IsRequired(false);
            builder.HasMany(x => x.ItemAttributes)
                .WithOne(x => x.Item)
                .HasForeignKey(x => x.ItemId);
        }
    }
}
