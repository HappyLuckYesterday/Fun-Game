using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class IncJobLevelPacket
{
    /// <summary>
    /// Gets the id.
    /// </summary>
    public byte Id { get; private set; }

    public IncJobLevelPacket(FFPacket packet)
    {
        Id = packet.ReadByte();
    }
}