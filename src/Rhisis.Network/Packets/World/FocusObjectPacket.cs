using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class FocusObjectPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the object id.
        /// </summary>
        public uint ObjectId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            ObjectId = packet.Read<uint>();
        }
    }
}