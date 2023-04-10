using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class PlayerAnglePacket
{
    /// <summary>
    /// Gets the Angle.
    /// </summary>
    public float Angle { get; private set; }

    /// <summary>
    /// Gets the X angle.
    /// </summary>
    public float AngleX { get; private set; }

    public PlayerAnglePacket(FFPacket packet)
    {
        Angle = packet.ReadSingle();
        AngleX = packet.ReadSingle();
    }
}