using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class SfxHitPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the SFX projectile id.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Gets the SFX projectile magic power.
    /// </summary>
    public int MagicPower { get; private set; }

    /// <summary>
    /// Gets the SFX projectile skill id.
    /// </summary>
    public int SkillId { get; private set; }

    /// <summary>
    /// Gets the SFX projectile caster id.
    /// </summary>
    public uint AttackerId { get; private set; }

    /// <summary>
    /// Gets the SFX projectile damage count.
    /// </summary>
    public uint DamageCount { get; private set; }

    /// <summary>
    /// Gets the SFX projectile damage angle.
    /// </summary>
    public float DamageAngle { get; private set; }

    /// <summary>
    /// Gets the SFX projectile damage power.
    /// </summary>
    public float DamagePower { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        Id = packet.ReadInt32();
        MagicPower = packet.ReadInt32();
        SkillId = packet.ReadInt32();
        AttackerId = packet.ReadUInt32();
        DamageCount = packet.ReadUInt32();
        DamageAngle = packet.ReadSingle();
        DamagePower = packet.ReadSingle();
    }
}