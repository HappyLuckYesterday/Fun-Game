using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World.Trade
{
    public class TradeCancelPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the trade cancel mode.
        /// </summary>
        public int Mode { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            Mode = packet.Read<int>();
        }
    }
}
