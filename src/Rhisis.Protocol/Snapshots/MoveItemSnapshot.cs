using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots
{
    public class MoveItemSnapshot : FFSnapshot
    {
        public MoveItemSnapshot(IPlayer player, int sourceSlot, int destinationSlot)
            : base(SnapshotType.MOVEITEM, player.Id)
        {
            WriteByte(0); // item type (not used)
            WriteByte((byte)sourceSlot);
            WriteByte((byte)destinationSlot);
        }
    }
}
