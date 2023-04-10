using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class QueryPlayerDataPacket
{
    /// <summary>
    /// Gets the player id.
    /// </summary>
    public uint PlayerId { get; private set; }

    /// <summary>
    /// Gets the version.
    /// </summary>
    public int Version { get; private set; }

    public QueryPlayerDataPacket(FFPacket packet)
    {
        PlayerId = packet.ReadUInt32();
        Version = packet.ReadInt32();
    }
}
