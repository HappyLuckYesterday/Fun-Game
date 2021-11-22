using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World.Party
{
    public class PartyChangeItemModePacket : IPacketDeserializer
    {

        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; private set; }

        /// <summary>
        /// Gets the item mode.
        /// </summary>
        public int ItemMode { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            PlayerId = packet.Read<uint>();
            ItemMode = packet.Read<int>();
        }
    }
}