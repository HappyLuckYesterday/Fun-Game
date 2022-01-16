using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World
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
        public void Deserialize(IFFPacket packet)
        {
            TargetId = packet.ReadUInt32();
            Message = packet.ReadString();
        }
    }
}