using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class NpcBuffPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the buffing NPC key.
        /// </summary>
        public string NpcKey { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            NpcKey = packet.ReadString();
        }
    }
}