using Rhisis.Game.Abstractions.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Snapshots
{
    public class SetHairSnapshot : FFSnapshot
    {
        public SetHairSnapshot(IPlayer player, byte hairId, byte r, byte g, byte b)
            : base(SnapshotType.SET_HAIR, player.Id)
        {
            Write(hairId);
            Write(r);
            Write(g);
            Write(b);
        }
    }
}
