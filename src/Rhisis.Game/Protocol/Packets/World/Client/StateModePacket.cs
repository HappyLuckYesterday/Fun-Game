using Rhisis.Game;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class StateModePacket
{
    /// <summary>
    /// Gets the client state mode.
    /// </summary>
    public StateMode StateMode { get; private set; }

    /// <summary>
    /// Gets the client state mode flag.
    /// </summary>
    public StateModeBaseMotion Flag { get; private set; }

    /// <inheritdoc />
    public StateModePacket(FFPacket packet)
    {
        StateMode = (StateMode)packet.ReadInt32();
        Flag = (StateModeBaseMotion)packet.ReadByte();
    }
}
