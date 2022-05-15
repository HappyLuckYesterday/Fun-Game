using Rhisis.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Snapshots
{
    public class UpdateItemSnapshot : FFSnapshot
    {
        public UpdateItemSnapshot(IPlayer player, UpdateItemType updateItemType, int itemIndex, int value)
            : base(SnapshotType.UPDATE_ITEM, player.Id)
        {
            WriteByte(0);
            WriteByte((byte)itemIndex);
            WriteByte((byte)updateItemType);
            WriteInt32(value);
            WriteInt32(0); // time
        }
    }
}
