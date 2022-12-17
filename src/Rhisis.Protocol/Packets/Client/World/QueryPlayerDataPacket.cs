using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class QueryPlayerDataPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the player id.
    /// </summary>
    public uint PlayerId { get; private set; }

    /// <summary>
    /// Gets the version.
    /// </summary>
    public int Version { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        PlayerId = packet.ReadUInt32();
        Version = packet.ReadInt32();
    }
}
