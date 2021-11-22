using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class PlayerDestObjectPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the target object id.
        /// </summary>
        public uint TargetObjectId { get; private set; }

        /// <summary>
        /// Gets the distance to the target.
        /// </summary>
        public float Distance { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            TargetObjectId = packet.Read<uint>();
            Distance = packet.Read<float>();
        }
    }
}