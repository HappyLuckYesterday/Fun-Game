using Rhisis.Protocol.Attributes;

namespace Rhisis.Protocol.Packets.Clients;

[PacketObject]
public sealed class CertifyPacket
{
    [PacketField(Order = 0)]
    public string BuildVersion { get; }

    [PacketField(Order = 1)]
    public string Username { get; }

    [PacketFieldSize(Size = 16 * 42)]
    public byte[] Password { get; }

    public CertifyPacket(FFPacket packet)
    {
        BuildVersion = packet.ReadString();
        Username = packet.ReadString();
        Password = packet.ReadBytes(16 * 42);
    }
}