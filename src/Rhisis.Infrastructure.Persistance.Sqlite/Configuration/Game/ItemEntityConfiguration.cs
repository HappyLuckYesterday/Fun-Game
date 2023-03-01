using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rhisis.Infrastructure.Persistance.Entities;

namespace Rhisis.Infrastructure.Persistance.Sqlite.Configuration.Game;

public sealed class ItemEntityConfiguration : IEntityTypeConfiguration<ItemEntity>
{
    public void Configure(EntityTypeBuilder<ItemEntity> builder)
    {
        builder.HasKey(x => x.SerialNumber);
        builder.Property(x => x.SerialNumber).IsRequired().ValueGeneratedOnAdd();
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Refine).IsRequired(false);
        builder.Property(x => x.Element).IsRequired(false);
        builder.Property(x => x.ElementRefine).IsRequired(false);
        builder.Property(x => x.OwnerId).IsRequired();
        builder.HasOne(x => x.Owner)
            .WithMany()
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}