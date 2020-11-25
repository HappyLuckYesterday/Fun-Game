using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rhisis.Database.Entities.Configuration
{
    public class ItemStorageConfiguration : IEntityTypeConfiguration<DbItemStorage>
    {
        public void Configure(EntityTypeBuilder<DbItemStorage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CharacterId).IsRequired();
            builder.Property(x => x.StorageTypeId).IsRequired();
            builder.Property(x => x.Slot).IsRequired().HasColumnType("SMALLINT");
            builder.Property(x => x.Quantity).IsRequired().HasColumnType("SMALLINT");
            builder.Property(x => x.Updated).IsRequired().HasColumnType("DATETIME").HasDefaultValueSql("NOW()");
            builder.Property(x => x.IsDeleted).IsRequired().HasColumnType("BIT").HasDefaultValue(false);

            builder.HasOne(x => x.Character)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.CharacterId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.StorageType)
                .WithMany()
                .HasForeignKey(x => x.StorageTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Item)
                .WithMany()
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
