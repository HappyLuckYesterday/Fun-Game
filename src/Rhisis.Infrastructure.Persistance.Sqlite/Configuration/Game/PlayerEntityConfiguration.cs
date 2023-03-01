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
        builder.Property(x => x.Experience).HasDefaultValue(0);
        builder.Property(x => x.JobId).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.Experience).HasDefaultValue(0);
        builder.Property(x => x.Slot).IsRequired();
        builder.Property(x => x.Strength).IsRequired();
        builder.Property(x => x.Stamina).IsRequired();
        builder.Property(x => x.Dexterity).IsRequired();
        builder.Property(x => x.Intelligence).IsRequired();
        builder.Property(x => x.Hp).IsRequired();
        builder.Property(x => x.Mp).IsRequired();
        builder.Property(x => x.Fp).IsRequired();
        builder.Property(x => x.SkinSetId).IsRequired();
        builder.Property(x => x.HairColor).IsRequired();
        builder.Property(x => x.HairId).IsRequired();
        builder.Property(x => x.FaceId).IsRequired();
        builder.Property(x => x.MapId).IsRequired();
        builder.Property(x => x.MapLayerId).IsRequired();
        builder.Property(x => x.PosX).IsRequired();
        builder.Property(x => x.PosY).IsRequired();
        builder.Property(x => x.PosZ).IsRequired();
        builder.Property(x => x.Angle).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.BankCode).IsRequired();
        builder.Property(x => x.StatPoints).IsRequired();
        builder.Property(x => x.SkillPoints).IsRequired();
        builder.Property(x => x.LastConnectionTime).IsRequired();
        builder.Property(x => x.PlayTime).IsRequired().HasDefaultValue(0);
        builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);
    }
}
