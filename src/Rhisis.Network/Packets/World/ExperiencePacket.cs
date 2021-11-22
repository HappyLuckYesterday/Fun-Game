using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class ExperiencePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the experience amount.
        /// </summary>
        public long Experience { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            Experience = packet.Read<long>();
        }
    }
}