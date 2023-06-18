using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rhisis.Game.Common;
using Rhisis.Infrastructure.Persistance.Entities;

namespace Rhisis.Infrastructure.Persistance.Sqlite.Configuration.Game;

public sealed class PlayerItemEntityConfiguration : IEntityTypeConfiguration<PlayerItemEntity>
{
    public void Configure(EntityTypeBuilder<PlayerItemEntity> builder)
    {
        builder.HasKey(x => new { x.PlayerId, x.StorageType, x.Slot });
        builder.HasIndex(x => new { x.PlayerId, x.StorageType, x.Slot }).IsUnique();
        builder.Property(x => x.PlayerId).IsRequired();
        builder.Property(x => x.ItemId).IsRequired();
        builder.Property(x => x.StorageType).IsRequired().HasColumnType("INTEGER")
            .HasConversion(
                x => (byte)x,
                x => (PlayerItemStorageType)x);
        builder.Property(x => x.Slot).IsRequired();
        builder.Property(x => x.Quantity);

        builder.HasOne(x => x.Item)
            .WithOne()
            .HasForeignKey<PlayerItemEntity>(x => x.ItemId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Player)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.PlayerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
