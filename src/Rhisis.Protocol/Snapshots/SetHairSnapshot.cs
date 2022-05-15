using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots
{
    public class SetHairSnapshot : FFSnapshot
    {
        public SetHairSnapshot(IPlayer player, byte hairId, byte r, byte g, byte b)
            : base(SnapshotType.SET_HAIR, player.Id)
        {
            WriteByte(hairId);
            WriteByte(r);
            WriteByte(g);
            WriteByte(b);
        }
    }
}
