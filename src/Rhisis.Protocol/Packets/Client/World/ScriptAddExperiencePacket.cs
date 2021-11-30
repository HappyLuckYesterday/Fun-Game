using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class ScriptAddExperiencePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public long Experience { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            Experience = packet.Read<long>();
        }
    }
}