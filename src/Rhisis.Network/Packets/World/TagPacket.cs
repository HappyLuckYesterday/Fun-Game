using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class TagPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the target id.
        /// </summary>
        public uint TargetId { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            TargetId = packet.Read<uint>();
            Message = packet.Read<string>();
        }
    }
}