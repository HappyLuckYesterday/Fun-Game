using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class CorrReqPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the object id.
    /// </summary>
    public uint ObjectId { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        ObjectId = packet.ReadUInt32();
    }
}