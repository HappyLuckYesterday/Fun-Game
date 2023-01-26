namespace Rhisis.Protocol.Packets;

public sealed class WelcomePacket : FFPacket
{
	public WelcomePacket(uint sessionId)
		: base(PacketType.WELCOME)
	{
		WriteUInt32(sessionId);
	}
}
