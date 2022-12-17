using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Trade;

public class TradeCancelPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the trade cancel mode.
    /// </summary>
    public int Mode { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        Mode = packet.ReadInt32();
    }
}
