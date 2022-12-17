using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class OpenShopWindowPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the selected object id.
    /// </summary>
    public uint ObjectId { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        ObjectId = packet.ReadUInt32();
    }
}
