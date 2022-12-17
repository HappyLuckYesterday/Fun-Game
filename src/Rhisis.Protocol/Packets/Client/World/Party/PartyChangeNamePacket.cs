using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Party;

public class PartyChangeNamePacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the player id.
    /// </summary>
    public uint PlayerId { get; private set; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        PlayerId = packet.ReadUInt32();
        Name = packet.ReadString();
    }
}