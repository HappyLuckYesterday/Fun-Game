using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class SfxIdPacket
{
    /// <summary>
    /// Gets the target id.
    /// </summary>
    public uint TargetId { get; private set; }

    /// <summary>
    /// Gets the id of the hit SFX.
    /// </summary>
    public int IdSfxHit { get; private set; }

    /// <summary>
    /// Gets the type.
    /// </summary>
    public uint Type { get; private set; }

    /// <summary>
    /// Gets the skill.
    /// </summary>
    public int Skill { get; private set; }

    /// <summary>
    /// Gets the max damage count.
    /// </summary>
    public int MaxDamageCount { get; private set; }

    public SfxIdPacket(FFPacket packet)
    {
        TargetId = packet.ReadUInt32();
        IdSfxHit = packet.ReadInt32();
        Type = packet.ReadUInt32();
        Skill = packet.ReadInt32();
        MaxDamageCount = packet.ReadInt32();
    }
}