using Rhisis.Core.Structures;
using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class DropItemPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the item type
    /// </summary>
    public uint ItemType { get; private set; }

    /// <summary>
    /// Gets the unique item id.
    /// </summary>
    public int ItemUniqueId { get; private set; }

    /// <summary>
    /// Gets the item quantity.
    /// </summary>
    public int ItemQuantity { get; private set; }

    /// <summary>
    /// Gets the position.
    /// </summary>
    public Vector3 Position { get; private set; }

    /// <inhertidoc />
    public void Deserialize(IFFPacket packet)
    {
        ItemType = packet.ReadUInt32();
        ItemUniqueId = packet.ReadInt32();
        ItemQuantity = packet.ReadInt16();
        Position = new Vector3(packet.ReadSingle(), packet.ReadSingle(), packet.ReadSingle());
    }
}