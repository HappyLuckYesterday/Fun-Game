using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Bank;

public class GetItemBankPacket
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

    public GetItemBankPacket(FFPacket packet)
    {
        Slot = packet.ReadByte();
        Id = packet.ReadByte();
        ItemNumber = packet.ReadInt16();
    }
}