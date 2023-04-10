using Rhisis.Game;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class LocalPosFromIAPacket
{
    /// <summary>
    /// Gets the local position.
    /// </summary>
    public Vector3 LocalPosition { get; private set; }

    /// <summary>
    /// Gets the id of the IA.
    /// </summary>
    public uint IAId { get; private set; }

    public LocalPosFromIAPacket(FFPacket packet)
    {
        LocalPosition = new Vector3(packet.ReadSingle(), packet.ReadSingle(), packet.ReadSingle());
        IAId = packet.ReadUInt32();
    }
}