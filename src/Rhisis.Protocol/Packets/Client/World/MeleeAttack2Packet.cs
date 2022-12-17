using Rhisis.Abstractions.Protocol;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Packets.Client.World;

public class MeleeAttack2Packet : IPacketDeserializer
{
    /// <summary>
    /// Gets the attack message.
    /// </summary>
    public ObjectMessageType AttackMessage { get; private set; }

    /// <summary>
    /// Gets the target object id.
    /// </summary>
    public uint ObjectId { get; private set; }

    /// <summary>
    /// Gets the second parameter.
    /// </summary>
    public int Parameter2 { get; private set; }

    /// <summary>
    /// Gets the third parameter.
    /// </summary>
    public int Parameter3 { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        AttackMessage = (ObjectMessageType)packet.ReadUInt32();
        ObjectId = packet.ReadUInt32();
        Parameter2 = packet.ReadInt32();
        Parameter3 = packet.ReadInt32();
    }
}