using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rhisis.Database.Entities.Configuration
{
    public class SkillBuffConfiguration : IEntityTypeConfiguration<DbSkillBuff>
    {
        public void Configure(EntityTypeBuilder<DbSkillBuff> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasIndex(x => new { x.CharacterId, x.SkillId }).IsUnique();
            builder.Property(x => x.SkillId).IsRequired();
            builder.Property(x => x.SkillLevel).IsRequired().HasColumnType("TINYINT");
            builder.Property(x => x.CharacterId).IsRequired();
            builder.HasOne(x => x.Character)
                .WithMany(x => x.SkillBuffs)
                .HasForeignKey(x => x.CharacterId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.RemainingTime).IsRequired();
        }
    }
}
