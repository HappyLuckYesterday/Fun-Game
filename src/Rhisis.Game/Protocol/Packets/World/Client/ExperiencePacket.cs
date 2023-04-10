using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class ExperiencePacket
{
    /// <summary>
    /// Gets the experience amount.
    /// </summary>
    public long Experience { get; private set; }

    public ExperiencePacket(FFPacket packet)
    {
        Experience = packet.ReadInt64();
    }
}