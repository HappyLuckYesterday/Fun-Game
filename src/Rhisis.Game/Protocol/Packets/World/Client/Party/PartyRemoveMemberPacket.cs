using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Party;

public class PartyRemoveMemberPacket
{

    public uint LeaderId { get; private set; }

    public uint MemberId { get; private set; }

    public PartyRemoveMemberPacket(FFPacket packet)
    {
        LeaderId = packet.ReadUInt32();
        MemberId = packet.ReadUInt32();
    }
}