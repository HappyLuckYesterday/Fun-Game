using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Bank
{
    public class GetItemBankPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the slot.
        /// </summary>
        public byte Slot { get; private set; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public byte Id { get; private set; }

        /// <summary>
        /// Gets the item number.
        /// </summary>
        public short ItemNumber { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            Slot = packet.ReadByte();
            Id = packet.ReadByte();
            ItemNumber = packet.ReadInt16();
        }
    }
}