using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class BuyChipItemPacket
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

    public BuyChipItemPacket(FFPacket packet)
    {
        Tab = packet.ReadByte();
        Id = packet.ReadByte();
        Quantity = packet.ReadInt16();
        ItemId = packet.ReadUInt32();
    }
}