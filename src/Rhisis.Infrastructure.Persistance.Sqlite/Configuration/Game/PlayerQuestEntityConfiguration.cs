using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rhisis.Infrastructure.Persistance.Entities;

namespace Rhisis.Infrastructure.Persistance.Sqlite.Configuration.Game;

public sealed class PlayerQuestEntityConfiguration : IEntityTypeConfiguration<PlayerQuestEntity>
{
    public void Configure(EntityTypeBuilder<PlayerQuestEntity> builder)
    {
        builder.HasKey(x => new { x.PlayerId, x.QuestId });
        builder.HasIndex(x => new { x.PlayerId, x.QuestId }).IsUnique();
        builder.Property(x => x.PlayerId).IsRequired();
        builder.Property(x => x.QuestId).IsRequired();
        builder.Property(x => x.Finished).IsRequired();
        builder.Property(x => x.IsChecked).IsRequired();
        builder.Property(x => x.IsDeleted).IsRequired();
        builder.Property(x => x.StartTime).IsRequired();
        builder.Property(x => x.IsPatrolDone).IsRequired();
        builder.Property(x => x.MonsterKilled1).IsRequired();
        builder.Property(x => x.MonsterKilled2).IsRequired();

        builder.HasOne(x => x.Player)
            .WithMany(x => x.Quests)
            .HasForeignKey(x => x.PlayerId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
