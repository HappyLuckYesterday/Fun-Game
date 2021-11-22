using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class ExpBoxInfoPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; private set; }

        /// <summary>
        /// Gets the object id.
        /// </summary>
        public uint ObjectId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            PlayerId = packet.Read<uint>();
            ObjectId = packet.Read<uint>();
        }
    }
}