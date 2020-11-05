using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Network.Snapshots
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
