using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Network.Snapshots
{
    public class UpdateItemSnapshot : FFSnapshot
    {
        public UpdateItemSnapshot(IPlayer player, UpdateItemType updateItemType, int itemIndex, int value)
            : base(SnapshotType.UPDATE_ITEM, player.Id)
        {
            Write<byte>(0);
            Write((byte)itemIndex);
            Write((byte)updateItemType);
            Write(value);
            Write(0); // time
        }
    }
}
