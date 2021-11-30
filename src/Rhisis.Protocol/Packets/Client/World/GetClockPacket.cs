using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class GetClockPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the SetBaseOfClient.
        /// </summary>
        public byte SetBaseOfClient { get; private set; }

        /// <summary>
        /// Gets the client time
        /// </summary>
        public uint ClientTime { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            SetBaseOfClient = packet.Read<byte>();
            ClientTime = packet.Read<uint>();
        }
    }
}