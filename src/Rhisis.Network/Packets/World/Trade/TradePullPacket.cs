using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World.Trade
{
    public class TradePullPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the slot.
        /// </summary>
        public byte Slot { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            Slot = packet.Read<byte>();
        }
    }
}