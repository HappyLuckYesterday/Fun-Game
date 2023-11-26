using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rhisis.Infrastructure.Persistance.Entities;
using System.ComponentModel;

namespace Rhisis.Infrastructure.Persistance.Sqlite.Configuration.Game;

public sealed class PlayerSkillEntityConfiguration : IEntityTypeConfiguration<PlayerSkillEntity>
{
    public void Configure(EntityTypeBuilder<PlayerSkillEntity> builder)
    {
        builder.HasKey(x => new { x.PlayerId, x.SkillId });
        builder.HasIndex(x => new { x.PlayerId, x.SkillId }).IsUnique();
        builder.Property(x => x.PlayerId).IsRequired();
        builder.Property(x => x.SkillId).IsRequired();
        builder.Property(x => x.SkillLevel).IsRequired();

        builder.HasOne(x => x.Player)
            .WithMany(x => x.Skills)
            .HasForeignKey(x => x.PlayerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
