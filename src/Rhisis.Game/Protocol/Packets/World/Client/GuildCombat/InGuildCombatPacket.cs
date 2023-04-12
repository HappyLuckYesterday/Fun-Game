using Rhisis.Game.Common;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.GuildCombat;

public class InGuildCombatPacket
{
    /// <summary>
    /// Gets the guild combat type.
    /// </summary>
    public GuildCombatType GuildCombatType { get; private set; }

    /// <summary>
    /// Gets the guild combat gold amount.
    /// </summary>
    public uint? Penya { get; private set; }

    public InGuildCombatPacket(FFPacket packet)
    {
        GuildCombatType = (GuildCombatType)packet.ReadInt32();
        Penya = GuildCombatType == GuildCombatType.GC_IN_APP ? packet.ReadUInt32() : null;
    }
}