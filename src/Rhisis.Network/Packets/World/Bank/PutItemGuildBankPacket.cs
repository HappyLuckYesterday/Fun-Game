using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World.Bank
{
    public class PutItemGuildBankPacket : IPacketDeserializer
    {

        public byte Id { get; private set; }

        public uint ItemId { get; private set; }

        public byte Mode { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            Id = packet.Read<byte>();
            ItemId = packet.Read<uint>();
            Mode = packet.Read<byte>();
        }
    }
}