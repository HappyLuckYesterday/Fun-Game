using Rhisis.Game.Common;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class MeleeAttack2Packet
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

    public MeleeAttack2Packet(FFPacket packet)
    {
        AttackMessage = (ObjectMessageType)packet.ReadUInt32();
        ObjectId = packet.ReadUInt32();
        Parameter2 = packet.ReadInt32();
        Parameter3 = packet.ReadInt32();
    }
}