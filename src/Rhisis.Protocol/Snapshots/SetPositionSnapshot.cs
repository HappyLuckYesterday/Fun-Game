using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots;

public class SetPositionSnapshot : FFSnapshot
{
    public SetPositionSnapshot(IPlayer player)
        : base(SnapshotType.SETPOS, player.Id)
    {
        WriteSingle(player.Position.X);
        WriteSingle(player.Position.Y);
        WriteSingle(player.Position.Z);
        WriteInt32(player.Map.Id);
    }
}
