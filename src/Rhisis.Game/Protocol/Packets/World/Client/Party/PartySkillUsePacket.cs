using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Party;

public class PartySkillUsePacket
{
    /// <summary>
    /// Gets the player id.
    /// </summary>
    public uint PlayerId { get; private set; }

    /// <summary>
    /// Gets the skill id.
    /// </summary>
    public int SkillId { get; private set; }

    public PartySkillUsePacket(FFPacket packet)
    {
        PlayerId = packet.ReadUInt32();
        SkillId = packet.ReadInt32();
    }
}