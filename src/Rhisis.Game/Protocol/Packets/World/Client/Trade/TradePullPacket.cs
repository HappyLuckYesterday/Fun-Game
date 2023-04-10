using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Trade;

public class TradePullPacket
{
    /// <summary>
    /// Gets the slot.
    /// </summary>
    public byte Slot { get; private set; }

    public TradePullPacket(FFPacket packet)
    {
        Slot = packet.ReadByte();
    }
}