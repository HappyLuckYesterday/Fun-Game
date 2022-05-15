using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class EquipItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the item index in the inventory.
        /// </summary>
        public int ItemIndex { get; private set; }

        /// <summary>
        /// Gets the equip item destination part.
        /// </summary>
        public int Part { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            ItemIndex = packet.ReadInt32();
            Part = packet.ReadInt32();
        }
    }
}
