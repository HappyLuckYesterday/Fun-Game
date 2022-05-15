using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Mailbox
{
    public class ReadMailPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the id of the mail
        /// </summary>
        public int MailId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            MailId = packet.ReadInt32();
        }
    }
}
