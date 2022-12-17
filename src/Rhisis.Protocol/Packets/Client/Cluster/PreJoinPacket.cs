using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.Cluster;

public class PreJoinPacket : IPacketDeserializer
{
    public string Username { get; private set; }

    public int CharacterId { get; private set; }

    public string CharacterName { get; private set; }

    public int BankCode { get; private set; }

    public void Deserialize(IFFPacket packet)
    {
        Username = packet.ReadString();
        CharacterId = packet.ReadInt32();
        CharacterName = packet.ReadString();
        BankCode = packet.ReadInt32();
    }
}
