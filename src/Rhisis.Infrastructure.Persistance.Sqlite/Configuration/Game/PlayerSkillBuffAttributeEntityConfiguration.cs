using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rhisis.Game.Common;
using Rhisis.Infrastructure.Persistance.Entities;
using System;

namespace Rhisis.Infrastructure.Persistance.Sqlite.Configuration.Game;

public sealed class PlayerSkillBuffAttributeEntityConfiguration : IEntityTypeConfiguration<PlayerSkillBuffAttributeEntity>
{
    public void Configure(EntityTypeBuilder<PlayerSkillBuffAttributeEntity> builder)
    {
        builder.HasKey(x => new { x.PlayerSkillBuffId, x.Attribute });
        builder.Property(x => x.PlayerSkillBuffId).IsRequired();
        builder.Property(x => x.Value).IsRequired();
        builder.Property(x => x.Attribute)
            .IsRequired()
            .HasColumnType("TEXT")
            .HasConversion(x => x.ToString(), x => (DefineAttributes)Enum.Parse(typeof(DefineAttributes), x));
        builder.HasOne(x => x.PlayerBuff)
            .WithMany(x => x.Attributes)
            .HasForeignKey(x => x.PlayerSkillBuffId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}