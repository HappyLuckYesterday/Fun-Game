using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rhisis.Database.Entities.Configuration
{
    public class FriendConfiguration : IEntityTypeConfiguration<DbFriend>
    {
        public void Configure(EntityTypeBuilder<DbFriend> builder)
        {
            builder.HasKey(x => x.CharacterId);
            builder.Property(x => x.CharacterId).IsRequired();
            builder.Property(x => x.FriendId).IsRequired();
            builder.Property(x => x.IsBlocked).IsRequired().HasDefaultValue(false).HasColumnType("BIT");
            builder.HasOne(x => x.Character)
                .WithMany(x => x.Friends)
                .HasForeignKey(x => x.CharacterId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Friend)
                .WithMany()
                .HasForeignKey(x => x.FriendId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
