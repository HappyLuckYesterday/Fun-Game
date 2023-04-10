using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Party;

public class PartyChangeLeaderPacket
{
    /// <summary>
    /// Gets the leader id.
    /// </summary>
    public uint LeaderId { get; private set; }

    /// <summary>
    /// Gets the new leader id.
    /// </summary>
    public uint NewLeaderId { get; private set; }

    public PartyChangeLeaderPacket(FFPacket packet)
    {
        LeaderId = packet.ReadUInt32();
        NewLeaderId = packet.ReadUInt32();
    }
}