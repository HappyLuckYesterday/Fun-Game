using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
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
        public void Deserialize(ILitePacketStream packet)
        {
            ItemIndex = packet.Read<byte>();
            Quantity = packet.Read<short>();
        }
    }
}
