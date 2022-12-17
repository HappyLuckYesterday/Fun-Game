namespace Rhisis.Abstractions.Protocol;

/// <summary>
/// Provides an interface to explore a flyff snapshot packet.
/// </summary>
public interface IFFSnapshot : IFFPacket
{
    /// <summary>
    /// Gets the amount of snapshots.
    /// </summary>
    short Count { get; }

    /// <summary>
    /// Merge the given snapshot into the current one.
    /// </summary>
    /// <param name="snapshot"></param>
    /// <returns></returns>
    IFFSnapshot Merge(IFFSnapshot snapshot);

    /// <summary>
    /// Gets the snapshot content as a byte array.
    /// </summary>
    /// <returns></returns>
    byte[] GetSnapshotContent();
}
