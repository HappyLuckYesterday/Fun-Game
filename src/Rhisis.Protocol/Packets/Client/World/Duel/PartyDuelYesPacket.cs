using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Duel
{
    public class PartyDuelYesPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the source player id.
        /// </summary>
        public uint SourcePlayerId { get; private set; }

        /// <summary>
        /// Gets the destination player id.
        /// </summary>
        public uint DestinationPlayerId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            SourcePlayerId = packet.ReadUInt32();
            DestinationPlayerId = packet.ReadUInt32();
        }
    }
}