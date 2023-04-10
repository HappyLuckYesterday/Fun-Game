using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Bank;

public class ChangeBankPasswordPacket
{
    /// <summary>
    /// Gets the old password.
    /// </summary>
    public string OldPassword { get; private set; }

    /// <summary>
    /// Gets the new password.
    /// </summary>
    public string NewPassword { get; private set; }

    /// <summary>
    /// Gets the id.
    /// </summary>
    public uint Id { get; private set; }

    /// <summary>
    /// Gets the item id.
    /// </summary>
    public uint ItemId { get; private set; }

    public ChangeBankPasswordPacket(FFPacket packet)
    {
        OldPassword = packet.ReadString();
        NewPassword = packet.ReadString();
        Id = packet.ReadUInt32();
        ItemId = packet.ReadUInt32();
    }
}