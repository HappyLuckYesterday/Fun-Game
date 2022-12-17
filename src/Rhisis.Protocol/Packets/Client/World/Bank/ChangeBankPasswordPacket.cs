using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Bank;

public class ChangeBankPasswordPacket : IPacketDeserializer
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

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        OldPassword = packet.ReadString();
        NewPassword = packet.ReadString();
        Id = packet.ReadUInt32();
        ItemId = packet.ReadUInt32();
    }
}