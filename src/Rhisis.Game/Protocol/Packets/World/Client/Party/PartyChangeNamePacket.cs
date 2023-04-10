using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Party;

public class PartyChangeNamePacket
{
    /// <summary>
    /// Gets the player id.
    /// </summary>
    public uint PlayerId { get; private set; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    public string Name { get; private set; }

    public PartyChangeNamePacket(FFPacket packet)
    {
        PlayerId = packet.ReadUInt32();
        Name = packet.ReadString();
    }
}