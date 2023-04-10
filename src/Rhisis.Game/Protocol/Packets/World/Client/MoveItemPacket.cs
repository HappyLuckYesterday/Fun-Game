using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class MoveItemPacket
{
    /// <summary>
    /// Gets the Item type.
    /// </summary>
    public byte ItemType { get; private set; }

    /// <summary>
    /// Gets the Item source slot.
    /// </summary>
    public byte SourceSlot { get; private set; }

    /// <summary>
    /// Gets the item destination slot.
    /// </summary>
    public byte DestinationSlot { get; private set; }

    public MoveItemPacket(FFPacket packet)
    {
        ItemType = packet.ReadByte();
        SourceSlot = packet.ReadByte();
        DestinationSlot = packet.ReadByte();
    }
}
