using Rhisis.Game.Common;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class MeleeAttackPacket
{
    /// <summary>
    /// Gets the attack message.
    /// </summary>
    public ObjectMessageType AttackMessage { get; private set; }

    /// <summary>
    /// Gets the object id.
    /// </summary>
    public uint ObjectId { get; private set; }

    /// <summary>
    /// Gets the unknown parameter.
    /// </summary>
    public int UnknownParameter { get; private set; }

    /// <summary>
    /// Gets the attack flags.
    /// </summary>
    public int AttackFlags { get; private set; }

    /// <summary>
    /// Gets the attack speed.
    /// </summary>
    public float WeaponAttackSpeed { get; private set; }

    public MeleeAttackPacket(FFPacket packet)
    {
        AttackMessage = (ObjectMessageType)packet.ReadInt32();
        ObjectId = packet.ReadUInt32();
        UnknownParameter = packet.ReadInt32(); // ??
        AttackFlags = packet.ReadInt32() & 0xFFFF; // Attack flags ?!
        WeaponAttackSpeed = packet.ReadSingle();
    }
}
