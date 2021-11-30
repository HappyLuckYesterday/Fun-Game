using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World.Party
{
    public class PartyChangeExperienceModePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; private set; }

        /// <summary>
        /// Gets the experience mode.
        /// </summary>
        public int ExperienceMode { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            PlayerId = packet.Read<uint>();
            ExperienceMode = packet.Read<int>();
        }
    }
}