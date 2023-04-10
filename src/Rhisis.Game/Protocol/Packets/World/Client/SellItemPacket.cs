using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class SellItemPacket
{
    /// <summary>
    /// Gets the item's index in the player's inventory.
    /// </summary>
    public byte ItemIndex { get; set; }

    /// <summary>
    /// Gets the item quantity to sell.
    /// </summary>
    public short Quantity { get; set; }

    public SellItemPacket(FFPacket packet)
    {
        ItemIndex = packet.ReadByte();
        Quantity = packet.ReadInt16();
    }
}
