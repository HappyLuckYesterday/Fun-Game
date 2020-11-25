using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rhisis.Database.Entities.Configuration
{
    public class SkillBuffAttributeConfiguration : IEntityTypeConfiguration<DbSkillBuffAttribute>
    {
        public void Configure(EntityTypeBuilder<DbSkillBuffAttribute> builder)
        {
            builder.HasKey(x => new { x.SkillBuffId, x.AttributeId });
            builder.HasIndex(x => new { x.SkillBuffId, x.AttributeId });
            builder.Property(x => x.SkillBuffId).IsRequired();
            builder.Property(x => x.AttributeId).IsRequired();
            builder.Property(x => x.Value).IsRequired();
            builder.HasOne(x => x.SkillBuff)
                .WithMany(x => x.Attributes)
                .HasForeignKey(x => x.SkillBuffId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Attribute)
                .WithMany()
                .HasForeignKey(x => x.AttributeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
