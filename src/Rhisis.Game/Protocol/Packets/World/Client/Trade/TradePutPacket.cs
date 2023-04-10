using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Trade;

public class TradePutPacket
{
    /// <summary>
    /// Gets the target position slot in the trade container.
    /// </summary>
    public byte Position { get; private set; }

    /// <summary>
    /// Gets the item type.
    /// </summary>
    public byte ItemType { get; private set; }

    /// <summary>
    /// Gets the item unique id.
    /// </summary>
    public byte ItemUniqueId { get; private set; }

    /// <summary>
    /// Gets the amount of items to put into the trade.
    /// </summary>
    public short Count { get; private set; }

    public TradePutPacket(FFPacket packet)
    {
        Position = packet.ReadByte();
        ItemType = packet.ReadByte();
        ItemUniqueId = packet.ReadByte();
        Count = packet.ReadInt16();
    }
}
