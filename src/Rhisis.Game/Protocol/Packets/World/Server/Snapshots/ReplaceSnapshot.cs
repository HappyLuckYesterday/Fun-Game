using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class ReplaceSnapshot : FFSnapshot
{
    public ReplaceSnapshot(Player player)
        : base(SnapshotType.REPLACE, player.ObjectId)
    {
        WriteInt32(player.Map.Id);
        WriteSingle(player.Position.X);
        WriteSingle(player.Position.Y);
        WriteSingle(player.Position.Z);
    }
}
