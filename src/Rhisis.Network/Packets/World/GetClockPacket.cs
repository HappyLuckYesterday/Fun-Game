using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
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
        public void Deserialize(ILitePacketStream packet)
        {
            SetBaseOfClient = packet.Read<byte>();
            ClientTime = packet.Read<uint>();
        }
    }
}