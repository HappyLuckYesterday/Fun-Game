using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class ScriptCreateItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the item type.
        /// </summary>
        public byte ItemType { get; private set; }

        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; private set; }

        /// <summary>
        /// Gets the quantity.
        /// </summary>
        public short Quantity { get; private set; }

        /// <summary>
        /// Gets the option.
        /// </summary>
        public int Option { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            ItemType = packet.Read<byte>();
            ItemId = packet.Read<uint>();
            Quantity = packet.Read<short>();
            Option = packet.Read<int>();
        }
    }
}