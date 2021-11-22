using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World.Party
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
        public void Deserialize(ILitePacketStream packet)
        {
            LeaderId = packet.Read<uint>();
            NewLeaderId = packet.Read<uint>();
        }
    }
}