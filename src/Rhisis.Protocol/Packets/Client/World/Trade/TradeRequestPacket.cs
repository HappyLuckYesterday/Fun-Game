using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World.Trade
{
    public sealed class TradeRequestPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the target object id for trade.
        /// </summary>
        public uint TargetId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            TargetId = packet.Read<uint>();
        }
    }
}
