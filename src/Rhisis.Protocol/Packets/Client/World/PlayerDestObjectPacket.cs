using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class PlayerDestObjectPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the target object id.
    /// </summary>
    public uint TargetObjectId { get; private set; }

    /// <summary>
    /// Gets the distance to the target.
    /// </summary>
    public float Distance { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        TargetObjectId = packet.ReadUInt32();
        Distance = packet.ReadSingle();
    }
}