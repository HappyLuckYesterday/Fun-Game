using Rhisis.Game.Common;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class SetTargetPacket
{
    /// <summary>
    /// Gets the target id.
    /// </summary>
    public uint TargetId { get; private set; }

    /// <summary>
    /// Gets a value indicating whether target should be cleared or not.
    /// </summary>
    public TargetModeType TargetMode { get; private set; }

    public SetTargetPacket(FFPacket packet)
    {
        TargetId = packet.ReadUInt32();
        TargetMode = (TargetModeType)packet.ReadByte();
    }
}