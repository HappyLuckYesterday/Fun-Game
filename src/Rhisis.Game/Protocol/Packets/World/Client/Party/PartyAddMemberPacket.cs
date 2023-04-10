using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Party;

public class PartyAddMemberPacket
{
    /// <summary>
    /// Gets the leader id.
    /// </summary>
    public uint LeaderId { get; private set; }

    /// <summary>
    /// Gets the leader level.
    /// </summary>
    public int LeaderLevel { get; private set; }

    /// <summary>
    /// Gets the leader job.
    /// </summary>
    public int LeaderJob { get; private set; }

    /// <summary>
    /// Gets the leader sex.
    /// </summary>
    public uint LeaderSex { get; private set; }

    /// <summary>
    /// Gets the member id.
    /// </summary>
    public uint MemberId { get; private set; }

    /// <summary>
    /// Gets the member level.
    /// </summary>
    public int MemberLevel { get; private set; }

    /// <summary>
    /// Gets the member job.
    /// </summary>
    public int MemberJob { get; private set; }

    /// <summary>
    /// Gets the member sex.
    /// </summary>
    public uint MemberSex { get; private set; }

    public PartyAddMemberPacket(FFPacket packet)
    {
        LeaderId = packet.ReadUInt32();
        LeaderLevel = packet.ReadInt32();
        LeaderJob = packet.ReadInt32();
        LeaderSex = packet.ReadUInt32();
        MemberId = packet.ReadUInt32();
        MemberLevel = packet.ReadInt32();
        MemberJob = packet.ReadInt32();
        MemberSex = packet.ReadUInt32();
    }
}