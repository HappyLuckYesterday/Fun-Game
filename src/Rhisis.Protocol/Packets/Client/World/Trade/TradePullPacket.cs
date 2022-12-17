using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Trade;

public class TradePullPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the slot.
    /// </summary>
    public byte Slot { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        Slot = packet.ReadByte();
    }
}