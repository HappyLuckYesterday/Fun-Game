using Rhisis.Abstractions.Protocol;

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
            ItemType = packet.ReadByte();
            ItemId = packet.ReadUInt32();
            Quantity = packet.ReadInt16();
            Option = packet.ReadInt32();
        }
    }
}