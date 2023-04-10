using Rhisis.Game;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class DropGoldPacket
{
    /// <summary>
    /// Gets the amount of gold.
    /// </summary>
    public uint Gold { get; private set; }

    /// <summary>
    /// Gets the position.
    /// </summary>
    public Vector3 Position { get; private set; }

    public DropGoldPacket(FFPacket packet)
    {
        Gold = packet.ReadUInt32();
        Position = new Vector3(packet.ReadSingle(), packet.ReadSingle(), packet.ReadSingle());
    }
}