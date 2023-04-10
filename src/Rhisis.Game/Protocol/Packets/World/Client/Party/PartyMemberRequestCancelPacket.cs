using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Party;

public class PartyMemberRequestCancelPacket
{
    /// <summary>
    /// Gets the leader id.
    /// </summary>
    public uint LeaderId { get; private set; }

    /// <summary>
    /// Gets the member id.
    /// </summary>
    public uint MemberId { get; private set; }

    /// <summary>
    /// Gets the mode.
    /// </summary>
    public int Mode { get; private set; }

    public PartyMemberRequestCancelPacket(FFPacket packet)
    {
        LeaderId = packet.ReadUInt32();
        MemberId = packet.ReadUInt32();
        Mode = packet.ReadInt32();
    }
}