using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Party;

public class PartyRemoveMemberPacket : IPacketDeserializer
{

    public uint LeaderId { get; private set; }

    public uint MemberId { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        LeaderId = packet.ReadUInt32();
        MemberId = packet.ReadUInt32();
    }
}