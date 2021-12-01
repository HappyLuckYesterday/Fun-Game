using Rhisis.Game.Abstractions.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Snapshots
{
    public class MoveItemSnapshot : FFSnapshot
    {
        public MoveItemSnapshot(IPlayer player, int sourceSlot, int destinationSlot)
            : base(SnapshotType.MOVEITEM, player.Id)
        {
            Write<byte>(0); // item type (not used)
            Write((byte)sourceSlot);
            Write((byte)destinationSlot);
        }
    }
}
