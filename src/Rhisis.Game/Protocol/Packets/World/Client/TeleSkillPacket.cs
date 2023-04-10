using Rhisis.Game;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class TeleSkillPacket
{
    /// <summary>
    /// Gets the position
    /// </summary>
    public Vector3 Position { get; private set; }

    /// <inheritdoc />
    public TeleSkillPacket(FFPacket packet)
    {
        Position = new Vector3(packet.ReadSingle(), packet.ReadSingle(), packet.ReadSingle());
    }
}