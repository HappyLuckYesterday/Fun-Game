using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class ModifyStatusPacket
{
    /// <summary>
    /// Gets the strength.
    /// </summary>
    public ushort Strength { get; private set; }

    /// <summary>
    /// Gets the stamina.
    /// </summary>
    public ushort Stamina { get; private set; }

    /// <summary>
    /// Gets the dexterity.
    /// </summary>
    public ushort Dexterity { get; private set; }

    /// <summary>
    /// Gets the intelligence.
    /// </summary>
    public ushort Intelligence { get; private set; }

    public ModifyStatusPacket(FFPacket packet)
    {
        Strength = (ushort)packet.ReadInt32();
        Stamina = (ushort)packet.ReadInt32();
        Dexterity = (ushort)packet.ReadInt32();
        Intelligence = (ushort)packet.ReadInt32();
    }
}
