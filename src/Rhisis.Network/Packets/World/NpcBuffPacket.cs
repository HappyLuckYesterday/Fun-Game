using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class NpcBuffPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the buffing NPC key.
        /// </summary>
        public string NpcKey { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            NpcKey = packet.ReadString();
        }
    }
}