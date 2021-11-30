using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World.Party
{
    public class PartyMemberRequestCancelPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the leader id.
        /// </summary>
        public uint LeaderId { get; private set; }

        /// <summary>
        /// Gets the member id.
        /// </summary>
        public uint MemberId { get; private set; }

        /// <summary>
        /// Gets the mode.
        /// </summary>
        public int Mode { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            LeaderId = packet.Read<uint>();
            MemberId = packet.Read<uint>();
            Mode = packet.Read<int>();
        }
    }
}