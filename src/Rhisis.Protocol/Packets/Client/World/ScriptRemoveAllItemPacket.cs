using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class ScriptRemoveAllItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the item id.
        /// </summary>
        public uint ItemId { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ScriptRemoveAllItemPacket"/> object.
        /// </summary>
        /// <param name="packet">Incoming packet</param>
        public void Deserialize(IFFPacket packet)
        {
            ItemId = packet.Read<uint>();
        }
    }
}