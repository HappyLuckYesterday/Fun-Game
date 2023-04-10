using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.GuildCombat;

public class SelectMapGuildCombatPacket
{
    /// <summary>
    /// Gets the map id.
    /// </summary>
    public int Map { get; private set; }

    public SelectMapGuildCombatPacket(FFPacket packet)
    {
        Map = packet.ReadInt32();
    }
}