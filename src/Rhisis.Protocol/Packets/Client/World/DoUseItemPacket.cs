using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class DoUseItemPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets or sets the item index in the inventory.
    /// </summary>
    public int ItemIndex { get; private set; }

    /// <summary>
    /// Gets or sets the object id.
    /// </summary>
    public int ObjectId { get; private set; }

    /// <summary>
    /// Gets or sets the body part.
    /// </summary>
    public int Part { get; private set; }

    /// <summary>
    /// Gets or sets the item flight speed.
    /// </summary>
    public float FlightSpeed { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        ItemIndex = packet.ReadInt32() >> 16 & 0xFFFF;
        ObjectId = packet.ReadInt32();
        Part = packet.ReadInt32();

        if (!packet.IsEndOfStream)
        {
            FlightSpeed = packet.ReadSingle();
        }
    }
}