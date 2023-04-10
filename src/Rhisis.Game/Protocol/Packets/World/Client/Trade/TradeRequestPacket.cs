using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Trade;

public sealed class TradeRequestPacket
{
    /// <summary>
    /// Gets the target object id for trade.
    /// </summary>
    public uint TargetId { get; private set; }

    public TradeRequestPacket(FFPacket packet)
    {
        TargetId = packet.ReadUInt32();
    }
}
