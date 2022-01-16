using Rhisis.Abstractions.Protocol;

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
            Id = packet.ReadByte();
            ItemId = packet.ReadUInt32();
            Mode = packet.ReadByte();
        }
    }
}