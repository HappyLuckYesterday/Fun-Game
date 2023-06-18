using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rhisis.Infrastructure.Persistance.Entities;

namespace Rhisis.Infrastructure.Persistance.Sqlite.Configuration.Game;

public sealed class PlayerSkillBuffEntityConfiguration : IEntityTypeConfiguration<PlayerSkillBuffEntity>
{
    public void Configure(EntityTypeBuilder<PlayerSkillBuffEntity> builder)
    {
        builder.HasIndex(x => new { x.PlayerId, x.SkillId }).IsUnique();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(x => x.PlayerId).IsRequired();
        builder.Property(x => x.SkillId).IsRequired();
        builder.Property(x => x.SkillLevel).IsRequired();
        builder.Property(x => x.RemainingTime).IsRequired();
        builder.HasMany(x => x.Attributes)
            .WithOne(x => x.PlayerBuff)
            .HasForeignKey(x => x.PlayerSkillBuffId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Player)
            .WithMany(x => x.Buffs)
            .HasForeignKey(x => x.PlayerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
