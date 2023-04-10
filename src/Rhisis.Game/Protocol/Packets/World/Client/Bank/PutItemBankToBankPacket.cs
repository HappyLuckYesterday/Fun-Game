using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Bank;

public class PutItemBankToBankPacket
{
    /// <summary>
    /// Gets the flag (always 1).
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
    /// Gets the id.
    /// </summary>
    public byte Id { get; }

    /// <summary>
    /// Gets the item number.
    /// </summary>
    public short ItemNumber { get; }

    public PutItemBankToBankPacket(FFPacket packet)
    {
        Flag = packet.ReadByte();
        DestinationSlot = packet.ReadByte();
        SourceSlot = packet.ReadByte();
        Id = packet.ReadByte();
        ItemNumber = packet.ReadInt16();
    }
}