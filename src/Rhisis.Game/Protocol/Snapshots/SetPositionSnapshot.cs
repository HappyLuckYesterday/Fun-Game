using Rhisis.Game.Abstractions.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Snapshots
{
    public class SetPositionSnapshot : FFSnapshot
    {
        public SetPositionSnapshot(IPlayer player)
            : base(SnapshotType.SETPOS, player.Id)
        {
            Write(player.Position.X);
            Write(player.Position.Y);
            Write(player.Position.Z);
            Write(player.Map.Id);
        }
    }
}
