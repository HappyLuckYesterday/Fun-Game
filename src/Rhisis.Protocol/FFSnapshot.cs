using System.IO;
using System.Linq;
using Rhisis.Core.Extensions;

namespace Rhisis.Protocol;

/// <summary>
/// Represents a FlyFF snapshot packet.
/// </summary>
public class FFSnapshot : FFPacket
{
    /// <summary>
    /// Gets the FlyFF snapshot header offset in the packet stream.
    /// </summary>
    /// <remarks>
    /// The snapshot header is located at offset 5.
    /// </remarks>
    public static readonly int SnapshotHeaderOffset = sizeof(byte) + sizeof(int);

    /// <summary>
    /// Gets the FlyFF snapshot amount value offset in the packet stream.
    /// </summary>
    /// <remarks>
    /// The snapshot amount value is located at offset 13.
    /// </remarks>
    public static readonly int SnapshotAmountOffset = SnapshotHeaderOffset + sizeof(int) + sizeof(int);

    /// <summary>
    /// Gets the FlyFF snapshot content value offset in the packet stream.
    /// </summary>
    public static readonly int SnapshotContentOffset = SnapshotAmountOffset + sizeof(short);

    /// <summary>
    /// Gets the number of packets inside the current snapshot.
    /// </summary>
    public short Count { get; private set; } = 0;

    public FFSnapshot()
        : base(PacketType.SNAPSHOT)
    {
        WriteInt32(0);
        WriteInt16(Count);
    }

    /// <summary>
    /// Creates a new <see cref="FFSnapshot"/> instance for a given object.
    /// </summary>
    /// <param name="snapshot">Snapshot type.</param>
    /// <param name="objectId">Target object id.</param>
    public FFSnapshot(SnapshotType snapshot, uint objectId)
        : base(PacketType.SNAPSHOT)
    {
        WriteInt32(0);
        WriteInt16(++Count);
        WriteUInt32(objectId);
        WriteInt16((short)((uint)snapshot));
    }

    /// <summary>
    /// Merge two snapshots together.
    /// </summary>
    /// <remarks>
    /// The given snapshot content is added at the end of the current one.
    /// </remarks>
    /// <param name="snapshot">Snapshot to merge.</param>
    /// <returns>Current snapshot.</returns>
    public FFSnapshot Merge(FFSnapshot snapshot)
    {
        Count += snapshot.Count;

        Seek(SnapshotAmountOffset, SeekOrigin.Begin);
        WriteInt16(Count);
        Seek(0, SeekOrigin.End);

        byte[] snapshotData = GetSnapshotContent(snapshot);

        Write(snapshotData, 0, snapshotData.Length);

        snapshot.Dispose();

        return this;
    }

    public byte[] GetContent() => GetSnapshotContent(this);

    private static byte[] GetSnapshotContent(FFSnapshot snapshot)
    {
        byte[] snapshotBuffer = snapshot.Buffer;

        return snapshotBuffer.GetRange(SnapshotContentOffset, snapshotBuffer.Length - SnapshotContentOffset).ToArray();
    }
}
