using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class ScriptCreateItemPacket
{
    /// <summary>
    /// Gets the item type.
    /// </summary>
    public byte ItemType { get; private set; }

    /// <summary>
    /// Gets the item id.
    /// </summary>
    public uint ItemId { get; private set; }

    /// <summary>
    /// Gets the quantity.
    /// </summary>
    public short Quantity { get; private set; }

    /// <summary>
    /// Gets the option.
    /// </summary>
    public int Option { get; private set; }

    public ScriptCreateItemPacket(FFPacket packet)
    {
        ItemType = packet.ReadByte();
        ItemId = packet.ReadUInt32();
        Quantity = packet.ReadInt16();
        Option = packet.ReadInt32();
    }
}