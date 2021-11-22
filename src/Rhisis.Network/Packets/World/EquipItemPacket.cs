using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
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
        public void Deserialize(ILitePacketStream packet)
        {
            ItemIndex = packet.Read<int>();
            Part = packet.Read<int>();
        }
    }
}
