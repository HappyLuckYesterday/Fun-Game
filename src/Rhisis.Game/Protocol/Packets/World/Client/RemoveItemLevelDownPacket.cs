using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class RemoveItemLevelDownPacket
{
    /// <summary>
    /// Gets the id.
    /// </summary>
    public uint Id { get; private set; }

    public RemoveItemLevelDownPacket(FFPacket packet)
    {
        Id = packet.ReadUInt32();
    }
}