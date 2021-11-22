using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class BuyChipItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the tab.
        /// </summary>
        public byte Tab { get; private set; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public byte Id { get; private set; }

        /// <summary>
        /// Gets the quantity.
        /// </summary>
        public short Quantity { get; private set; }

        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            Tab = packet.Read<byte>();
            Id = packet.Read<byte>();
            Quantity = packet.Read<short>();
            ItemId = packet.Read<uint>();
        }
    }
}