using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class BuyChipItemPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the tab.
    /// </summary>
    public byte Tab { get; private set; }

    /// <summary>
    /// Gets the id.
    /// </summary>
    public byte Id { get; private set; }

    /// <summary>
    /// Gets the quantity.
    /// </summary>
    public short Quantity { get; private set; }

    /// <summary>
    /// Gets the item id.
    /// </summary>
    public uint ItemId { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        Tab = packet.ReadByte();
        Id = packet.ReadByte();
        Quantity = packet.ReadInt16();
        ItemId = packet.ReadUInt32();
    }
}