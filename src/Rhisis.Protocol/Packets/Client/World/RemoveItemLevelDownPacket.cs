using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class RemoveItemLevelDownPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the id.
    /// </summary>
    public uint Id { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        Id = packet.ReadUInt32();
    }
}