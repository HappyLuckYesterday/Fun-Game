using Microsoft.Extensions.Options;
using Rhisis.Abstractions.Protocol;
using Rhisis.Core.Cryptography;
using Rhisis.Core.Structures.Configuration;

namespace Rhisis.Protocol.Packets.Client.Login
{
    public class CertifyPacket : IPacketDeserializer
    {
        private readonly LoginConfiguration _configuration;

        public virtual string BuildVersion { get; private set; }

        public virtual string Username { get; private set; }

        public virtual string Password { get; private set; }

        public CertifyPacket(IOptions<LoginConfiguration> loginConfiguration)
        {
            _configuration = loginConfiguration.Value;
        }

        public void Deserialize(IFFPacket packet)
        {
            BuildVersion = packet.ReadString();
            Username = packet.ReadString();
            Password = null;

            if (_configuration.PasswordEncryption)
            {
                byte[] encryptedPassword = packet.ReadBytes(16 * 42);
                byte[] encryptionKey = Aes.BuildEncryptionKeyFromString(_configuration.EncryptionKey, 16);

                Password = Aes.DecryptByteArray(encryptedPassword, encryptionKey);
            }
            else
            {
                Password = packet.ReadString();
            }
        }
    }
}