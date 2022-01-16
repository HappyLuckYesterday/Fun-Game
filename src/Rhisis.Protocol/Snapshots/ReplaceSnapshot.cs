using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots
{
    public class ReplaceSnapshot : FFSnapshot
    {
        public ReplaceSnapshot(IPlayer player)
            : base(SnapshotType.REPLACE, player.Id)
        {
            Write(player.Map.Id);
            Write(player.Position.X);
            Write(player.Position.Y);
            Write(player.Position.Z);
        }
    }
}
