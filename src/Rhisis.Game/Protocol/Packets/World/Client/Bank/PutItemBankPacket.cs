using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Bank;

public class PutItemBankPacket
{
    /// <summary>
    /// Gets the slot.
    /// </summary>
    public byte Slot { get; }

    /// <summary>
    /// Gets the id.
    /// </summary>
    public byte Id { get; }

    /// <summary>
    /// Gets the item number.
    /// </summary>
    public short ItemNumber { get; }

    public PutItemBankPacket(FFPacket packet)
    {
        Slot = packet.ReadByte();
        Id = packet.ReadByte();
        ItemNumber = packet.ReadInt16();
    }
}