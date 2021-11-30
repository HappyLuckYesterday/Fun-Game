using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World.Bank
{
    public class PutGoldBankToBankPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the flag (always 0).
        /// </summary>
        public byte Flag { get; private set; }

        /// <summary>
        /// Gets the destination slot.
        /// </summary>
        public byte DestinationSlot { get; private set; }

        /// <summary>
        /// Gets the source slot.
        /// </summary>
        public byte SourceSlot { get; private set; }

        /// <summary>
        /// Gets the amount of gold.
        /// </summary>
        public uint Gold { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            Flag = packet.Read<byte>();
            DestinationSlot = packet.Read<byte>();
            SourceSlot = packet.Read<byte>();
            Gold = packet.Read<uint>();
        }
    }
}