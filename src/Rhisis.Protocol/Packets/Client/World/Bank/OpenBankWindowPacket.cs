using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Bank;

public class OpenBankWindowPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the id.
    /// </summary>
    public uint Id { get; private set; }

    /// <summary>
    /// Gets the item id.
    /// </summary>
    public uint ItemId { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        Id = packet.ReadUInt32();
        ItemId = packet.ReadUInt32();
    }
}