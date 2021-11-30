using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class SellItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the item's index in the player's inventory.
        /// </summary>
        public byte ItemIndex { get; set; }

        /// <summary>
        /// Gets the item quantity to sell.
        /// </summary>
        public short Quantity { get; set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            ItemIndex = packet.Read<byte>();
            Quantity = packet.Read<short>();
        }
    }
}
