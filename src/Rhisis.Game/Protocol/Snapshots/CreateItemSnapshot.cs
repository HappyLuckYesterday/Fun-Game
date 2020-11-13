using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Network.Snapshots
{
    public class CreateItemSnapshot : FFSnapshot
    {
        public CreateItemSnapshot(IPlayer player, IItem item)
            : base(SnapshotType.CREATEITEM, player.Id)
        {
            Write<byte>(0);
            item.Serialize(this, -1);
            Write<byte>(1);
            Write((byte)item.Index);
            Write((short)item.Quantity);
        }
    }
}
