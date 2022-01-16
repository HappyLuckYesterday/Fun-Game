using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Party
{
    public class PartyChangeLeaderPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the leader id.
        /// </summary>
        public uint LeaderId { get; private set; }

        /// <summary>
        /// Gets the new leader id.
        /// </summary>
        public uint NewLeaderId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            LeaderId = packet.ReadUInt32();
            NewLeaderId = packet.ReadUInt32();
        }
    }
}