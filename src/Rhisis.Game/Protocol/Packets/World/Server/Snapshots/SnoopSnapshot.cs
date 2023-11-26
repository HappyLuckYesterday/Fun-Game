using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class SnoopSnapshot : FFSnapshot
{
    public SnoopSnapshot(string text)
        : base(SnapshotType.SNOOP, 0)
    {
        WriteString(text);
    }
}
