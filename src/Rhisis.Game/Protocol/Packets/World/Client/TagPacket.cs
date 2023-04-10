using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class TagPacket
{
    /// <summary>
    /// Gets the target id.
    /// </summary>
    public uint TargetId { get; private set; }

    /// <summary>
    /// Gets the message.
    /// </summary>
    public string Message { get; private set; }

    public TagPacket(FFPacket packet)
    {
        TargetId = packet.ReadUInt32();
        Message = packet.ReadString();
    }
}