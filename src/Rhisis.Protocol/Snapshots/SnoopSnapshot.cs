namespace Rhisis.Protocol.Snapshots;

public class SnoopSnapshot : FFSnapshot
{
    public SnoopSnapshot(string text)
        : base(SnapshotType.SNOOP, 0)
    {
        WriteString(text);
    }
}
