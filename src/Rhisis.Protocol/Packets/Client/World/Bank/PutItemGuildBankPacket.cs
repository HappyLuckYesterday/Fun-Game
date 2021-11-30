using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World.Bank
{
    public class PutItemGuildBankPacket : IPacketDeserializer
    {

        public byte Id { get; private set; }

        public uint ItemId { get; private set; }

        public byte Mode { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            Id = packet.Read<byte>();
            ItemId = packet.Read<uint>();
            Mode = packet.Read<byte>();
        }
    }
}