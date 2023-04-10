using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class DoUseItemTargetPacket
{
    /// <summary>
    /// Gets the material.
    /// </summary>
    public uint Material { get; private set; }

    /// <summary>
    /// Gets the target.
    /// </summary>
    public uint Target { get; private set; }

    public DoUseItemTargetPacket(FFPacket packet)
    {
        Material = packet.ReadUInt32();
        Target = packet.ReadUInt32();
    }
}