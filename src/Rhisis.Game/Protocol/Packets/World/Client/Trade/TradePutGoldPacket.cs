using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Trade;

public class TradePutGoldPacket
{
    /// <summary>
    /// Gets the amount of gold the player has placed in the trade.
    /// </summary>
    public int Gold { get; private set; }

    public TradePutGoldPacket(FFPacket packet)
    {
        Gold = packet.ReadInt32();
    }
}
