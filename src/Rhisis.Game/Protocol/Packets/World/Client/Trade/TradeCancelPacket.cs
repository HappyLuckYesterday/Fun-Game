using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Trade;

public class TradeCancelPacket
{
    /// <summary>
    /// Gets the trade cancel mode.
    /// </summary>
    public int Mode { get; private set; }

    public TradeCancelPacket(FFPacket packet)
    {
        Mode = packet.ReadInt32();
    }
}
