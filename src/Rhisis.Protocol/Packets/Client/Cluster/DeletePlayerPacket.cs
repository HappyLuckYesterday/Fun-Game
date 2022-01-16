using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.Cluster
{
    public class DeletePlayerPacket : IPacketDeserializer
    {
        public string Username { get; private set; }

        public string Password { get; private set; }

        public string PasswordConfirmation { get; private set; }

        public int CharacterId { get; private set; }

        public int AuthenticationKey { get; private set; }

        public void Deserialize(IFFPacket packet)
        {
            Username = packet.ReadString();
            Password = packet.ReadString();
            PasswordConfirmation = packet.ReadString();
            CharacterId = packet.ReadInt32();
            AuthenticationKey = packet.ReadInt32();
        }
    }
}
