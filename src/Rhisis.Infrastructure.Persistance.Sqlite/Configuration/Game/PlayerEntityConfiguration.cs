using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rhisis.Infrastructure.Persistance.Entities;

namespace Rhisis.Infrastructure.Persistance.Sqlite.Configuration.Game;

public sealed class PlayerEntityConfiguration : IEntityTypeConfiguration<PlayerEntity>
{
    public void Configure(EntityTypeBuilder<PlayerEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(x => x.AccountId).IsRequired();
        builder.Property(x => x.Name).IsRequired().HasColumnType("TEXT").HasMaxLength(32);
        builder.Property(x => x.Level).IsRequired();
    }
}
