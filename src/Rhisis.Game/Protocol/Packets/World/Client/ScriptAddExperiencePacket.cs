using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class ScriptAddExperiencePacket
{
    /// <summary>
    /// Gets the id.
    /// </summary>
    public long Experience { get; private set; }

    public ScriptAddExperiencePacket(FFPacket packet)
    {
        Experience = packet.ReadInt64();
    }
}