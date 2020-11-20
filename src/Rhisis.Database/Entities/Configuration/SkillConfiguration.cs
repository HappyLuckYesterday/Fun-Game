using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rhisis.Database.Entities.Configuration
{
    public class SkillConfiguration : IEntityTypeConfiguration<DbSkill>
    {
        public void Configure(EntityTypeBuilder<DbSkill> builder)
        {
            builder.HasIndex(x => new { x.SkillId, x.CharacterId }).IsUnique();
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.SkillId).IsRequired();
            builder.Property(x => x.Level).IsRequired().HasColumnType("TINYINT");
            builder.Property(x => x.CharacterId).IsRequired();
            builder.HasOne(x => x.Character)
                .WithMany(x => x.Skills)
                .HasForeignKey(x => x.CharacterId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
