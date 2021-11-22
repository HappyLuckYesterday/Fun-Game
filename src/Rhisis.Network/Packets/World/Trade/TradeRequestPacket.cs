using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World.Trade
{
    public sealed class TradeRequestPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the target object id for trade.
        /// </summary>
        public uint TargetId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            TargetId = packet.Read<uint>();
        }
    }
}
