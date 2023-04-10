using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Bank;

public class GetItemGuildBankPacket
{
    /// <summary>
    /// Gets the id.
    /// </summary>
    public byte Id { get; }

    /// <summary>
    /// Gets the item id.
    /// </summary>
    public uint ItemId { get; }

    /// <summary>
    /// Gets the mode.
    /// </summary>
    public byte Mode { get; }

    /// <inheritdoc />
    public GetItemGuildBankPacket(FFPacket packet)
    {
        Id = packet.ReadByte();
        ItemId = packet.ReadUInt32();
        Mode = packet.ReadByte();
    }
}