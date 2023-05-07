using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class SetPositionSnapshot : FFSnapshot
{
    public SetPositionSnapshot(Player player)
        : base(SnapshotType.SETPOS, player.ObjectId)
    {
        WriteSingle(player.Position.X);
        WriteSingle(player.Position.Y);
        WriteSingle(player.Position.Z);
        WriteInt32(player.Map.Id);
    }
}
