using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class BlessednessCancelPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the item id.
    /// </summary>
    public int Item { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        Item = packet.ReadInt32();
    }
}