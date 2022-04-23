using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots
{
    public class ReplaceSnapshot : FFSnapshot
    {
        public ReplaceSnapshot(IPlayer player)
            : base(SnapshotType.REPLACE, player.Id)
        {
            WriteInt32(player.Map.Id);
            WriteSingle(player.Position.X);
            WriteSingle(player.Position.Y);
            WriteSingle(player.Position.Z);
        }
    }
}
