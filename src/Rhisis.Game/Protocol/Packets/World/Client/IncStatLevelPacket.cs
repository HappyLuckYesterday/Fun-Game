using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class IncStatLevelPacket
{
    /// <summary>
    /// Gets the id.
    /// </summary>
    public byte Id { get; private set; }

    public IncStatLevelPacket(FFPacket packet)
    {
        Id = packet.ReadByte();
    }
}