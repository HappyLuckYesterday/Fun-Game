using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.GuildCombat;

public class SelectPlayerGuildCombatPacket
{
    /// <summary>
    /// Gets the window.
    /// </summary>
    public bool Window { get; private set; }

    /// <summary>
    /// Gets the defender id.
    /// </summary>
    public uint? DefenderId { get; private set; }

    /// <summary>
    /// Gets the SelectPlayer size.
    /// </summary>
    public int? Size { get; private set; }

    /// <summary>
    /// Gets the players to select.
    /// </summary>
    public uint?[] SelectPlayer { get; private set; }

    public SelectPlayerGuildCombatPacket(FFPacket packet)
    {
        Window = packet.ReadInt32() == 1;
        if (!Window)
        {
            DefenderId = packet.ReadUInt32();
            Size = packet.ReadInt32();
            SelectPlayer = new uint?[Size.Value];
            for (int i = 0; i < Size.Value; i++)
                SelectPlayer[i] = packet.ReadUInt32();
        }
        else
        {
            DefenderId = null;
            Size = null;
            SelectPlayer = null;
        }
    }
}