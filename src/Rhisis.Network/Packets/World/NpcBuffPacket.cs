using Rhisis.Game.Abstractions.Protocol;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class NpcBuffPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the buffing NPC key.
        /// </summary>
        public string NpcKey { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            NpcKey = packet.ReadString();
        }
    }
}