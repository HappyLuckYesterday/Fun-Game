using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Bank;

public class OpenBankWindowPacket
{
    /// <summary>
    /// Gets the id.
    /// </summary>
    public uint Id { get; }

    /// <summary>
    /// Gets the item id.
    /// </summary>
    public uint ItemId { get; }

    /// <inheritdoc />
    public OpenBankWindowPacket(FFPacket packet)
    {
        Id = packet.ReadUInt32();
        ItemId = packet.ReadUInt32();
    }
}