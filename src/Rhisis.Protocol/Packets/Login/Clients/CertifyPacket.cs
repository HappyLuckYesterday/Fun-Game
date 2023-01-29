namespace Rhisis.Protocol.Packets.Login.Clients;

public sealed class CertifyPacket
{
    public string BuildVersion { get; }

    public string Username { get; }

    public byte[] Password { get; }

    public CertifyPacket(FFPacket packet)
    {
        BuildVersion = packet.ReadString();
        Username = packet.ReadString();
        Password = packet.ReadBytes(16 * 42);
    }
}
