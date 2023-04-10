using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Bank;

public class PutGoldBankToBankPacket
{
    /// <summary>
    /// Gets the flag (always 0).
    /// </summary>
    public byte Flag { get; }

    /// <summary>
    /// Gets the destination slot.
    /// </summary>
    public byte DestinationSlot { get; }

    /// <summary>
    /// Gets the source slot.
    /// </summary>
    public byte SourceSlot { get; }

    /// <summary>
    /// Gets the amount of gold.
    /// </summary>
    public uint Gold { get; }

    /// <inheritdoc />
    public PutGoldBankToBankPacket(FFPacket packet)
    {
        Flag = packet.ReadByte();
        DestinationSlot = packet.ReadByte();
        SourceSlot = packet.ReadByte();
        Gold = packet.ReadUInt32();
    }
}