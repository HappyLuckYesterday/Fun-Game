using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Bank;

public class GetGoldBankPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the slot.
    /// </summary>
    public byte Slot { get; private set; }

    /// <summary>
    /// Gets the amount of gold.
    /// </summary>
    public uint Gold { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        Slot = packet.ReadByte();
        Gold = packet.ReadUInt32();
    }
}