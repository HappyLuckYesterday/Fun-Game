using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.Cluster;

public class GetPlayerListPacket : IPacketDeserializer
{
    public string BuildVersion { get; private set; }

    public int AuthenticationKey { get; private set; }

    public string Username { get; private set; }

    public string Password { get; private set; }

    public int ServerId { get; private set; }

    public void Deserialize(IFFPacket packet)
    {
        BuildVersion = packet.ReadString();
        AuthenticationKey = packet.ReadInt32();
        Username = packet.ReadString();
        Password = packet.ReadString();
        ServerId = packet.ReadInt32();
    }
}
