using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.Cluster.Client;

public class QueryTickCountPacket
{
    /// <summary>
    /// Gets the last elapsed time.
    /// </summary>
    public uint Time { get; }

    public QueryTickCountPacket(FFPacket packet)
    {
        Time = packet.ReadUInt32();
    }
}
