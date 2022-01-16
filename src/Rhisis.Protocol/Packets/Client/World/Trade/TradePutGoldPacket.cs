using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Trade
{
    public class TradePutGoldPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the amount of gold the player has placed in the trade.
        /// </summary>
        public int Gold { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            Gold = packet.ReadInt32();
        }
    }
}
