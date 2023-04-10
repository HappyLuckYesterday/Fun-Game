using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class ErrorPacket
{
    /// <summary>
    /// Gets the code.
    /// </summary>
    public int Code { get; private set; }

    /// <summary>
    /// Gets the data.
    /// </summary>
    public int Data { get; private set; }

    public ErrorPacket(FFPacket packet)
    {
        Code = packet.ReadInt32();
        Data = packet.ReadInt32();
    }
}