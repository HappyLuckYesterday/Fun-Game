using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Bank;

public class PutItemBankToBankPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the flag (always 1).
    /// </summary>
    public byte Flag { get; private set; }

    /// <summary>
    /// Gets the destination slot.
    /// </summary>
    public byte DestinationSlot { get; private set; }

    /// <summary>
    /// Gets the source slot.
    /// </summary>
    public byte SourceSlot { get; private set; }

    /// <summary>
    /// Gets the id.
    /// </summary>
    public byte Id { get; private set; }

    /// <summary>
    /// Gets the item number.
    /// </summary>
    public short ItemNumber { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        Flag = packet.ReadByte();
        DestinationSlot = packet.ReadByte();
        SourceSlot = packet.ReadByte();
        Id = packet.ReadByte();
        ItemNumber = packet.ReadInt16();
    }
}