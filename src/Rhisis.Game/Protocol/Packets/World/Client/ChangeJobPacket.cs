using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class ChangeJobPacket
{
    /// <summary>
    /// Gets the job.
    /// </summary>
    public int Job { get; private set; }

    /// <summary>
    /// I have no idea what gama is. Probably game?
    /// </summary>
    public bool Gama { get; private set; }

    public ChangeJobPacket(FFPacket packet)
    {
        Job = packet.ReadInt32();
        Gama = packet.ReadInt32() == 1;
    }
}