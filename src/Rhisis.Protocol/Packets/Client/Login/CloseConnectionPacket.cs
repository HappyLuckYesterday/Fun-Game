using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.Login
{
    public class CloseConnectionPacket : IPacketDeserializer
    {
        public string Username { get; private set; }

        public string Password { get; private set; }

        public void Deserialize(IFFPacket packet)
        {
            Username = packet.Read<string>();
            Password = packet.Read<string>();
        }
    }
}