using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World.Duel
{
    public class DuelNoPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            PlayerId = packet.Read<uint>();
        }
    }
}