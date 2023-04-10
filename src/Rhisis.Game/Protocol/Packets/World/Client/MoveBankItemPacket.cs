using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class MoveBankItemPacket
{
    /// <summary>
    /// Gets the source index.
    /// </summary>
    public int SourceIndex { get; private set; }

    /// <summary>
    /// Gets the destination index.
    /// </summary>
    public int DestinationIndex { get; private set; }

    public MoveBankItemPacket(FFPacket packet)
    {
        SourceIndex = packet.ReadInt32();
        DestinationIndex = packet.ReadInt32();
    }
}