using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Mailbox
{
    public class QueryPostMailPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the slot of the item.
        /// </summary>
        public byte ItemSlot { get; private set; }

        /// <summary>
        /// Gets the quantity of the item.
        /// </summary>
        public short ItemQuantity { get; private set; }

        /// <summary>
        /// Gets the receiver's name.
        /// </summary>
        public string Receiver { get; private set; }

        /// <summary>
        /// Gets the amount of gold.
        /// </summary>
        public int Gold { get; private set; }

        /// <summary>
        /// Gets the title of the mail.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the text of the mail.
        /// </summary>
        public string Text { get; private set; }


        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            ItemSlot = packet.ReadByte();
            ItemQuantity = packet.ReadInt16();
            Receiver = packet.ReadString();
            Gold = packet.ReadInt32();
            Title = packet.ReadString();
            Text = packet.ReadString();
        }
    }
}
