using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class GetClockPacket
{
    /// <summary>
    /// Gets the SetBaseOfClient.
    /// </summary>
    public byte SetBaseOfClient { get; private set; }

    /// <summary>
    /// Gets the client time
    /// </summary>
    public uint ClientTime { get; private set; }

    public GetClockPacket(FFPacket packet)
    {
        SetBaseOfClient = packet.ReadByte();
        ClientTime = packet.ReadUInt32();
    }
}