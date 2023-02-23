namespace Rhisis.Protocol.Packets.Cluster.Server;

public class LoginProtectCertPacket : FFPacket
{
    public LoginProtectCertPacket(int loginProtectValue)
        : base(PacketType.LOGIN_PROTECT_CERT)
    {
        WriteInt32(0);
        WriteInt32(loginProtectValue);
    }
}
