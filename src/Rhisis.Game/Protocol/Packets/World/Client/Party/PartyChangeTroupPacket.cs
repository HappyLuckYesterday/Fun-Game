using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Party;

public class PartyChangeTroupPacket
{
    /// <summary>
    /// Gets the player id.
    /// </summary>
    public uint PlayerId { get; private set; }

    /// <summary>
    /// Gets if a name was sent.
    /// </summary>
    public bool SendName { get; private set; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; private set; }

    public PartyChangeTroupPacket(FFPacket packet)
    {
        PlayerId = packet.ReadUInt32();
        SendName = packet.ReadInt32() == 1;
        Name = SendName ? packet.ReadString() : null;
    }
}