using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Bank;

public class PutItemGuildBankPacket
{
    public byte Id { get; }

    public uint ItemId { get; }

    public byte Mode { get; }

    public PutItemGuildBankPacket(FFPacket packet)
    {
        Id = packet.ReadByte();
        ItemId = packet.ReadUInt32();
        Mode = packet.ReadByte();
    }
}