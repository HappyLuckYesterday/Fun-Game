namespace Rhisis.Protocol.Packets.Server.Cluster;

public class LoginProctectNumPadPacket : FFPacket
{
    public LoginProctectNumPadPacket(int loginProtectValue)
        : base(PacketType.LOGIN_PROTECT_NUMPAD)
    {
        WriteInt32(loginProtectValue);
    }
}
