using Rhisis.Abstractions;
using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots
{
    public class CreateItemSnapshot : FFSnapshot
    {
        public CreateItemSnapshot(IPlayer player, IItem item)
            : base(SnapshotType.CREATEITEM, player.Id)
        {
            WriteByte(0);
            item.Serialize(this, -1);
            WriteByte(1);
            WriteByte((byte)item.Index);
            WriteInt16((short)item.Quantity);
        }
    }
}
