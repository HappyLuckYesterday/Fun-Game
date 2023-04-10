using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Party;

public class PartyChangeExperienceModePacket
{
    /// <summary>
    /// Gets the player id.
    /// </summary>
    public uint PlayerId { get; private set; }

    /// <summary>
    /// Gets the experience mode.
    /// </summary>
    public int ExperienceMode { get; private set; }

    public PartyChangeExperienceModePacket(FFPacket packet)
    {
        PlayerId = packet.ReadUInt32();
        ExperienceMode = packet.ReadInt32();
    }
}