using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class ScriptEquipItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; private set; }

        /// <summary>
        /// Gets the option.
        /// </summary>
        public int Option { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            ItemId = packet.Read<uint>();
            Option = packet.Read<int>();
        }
    }
}