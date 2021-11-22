using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class ErrorPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the code.
        /// </summary>
        public int Code { get; private set; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        public int Data { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            Code = packet.Read<int>();
            Data = packet.Read<int>();
        }
    }
}