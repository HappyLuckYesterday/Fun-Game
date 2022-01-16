using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class ExperiencePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the experience amount.
        /// </summary>
        public long Experience { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            Experience = packet.ReadInt64();
        }
    }
}