using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class ExpBoxInfoPacket
{
    /// <summary>
    /// Gets the player id.
    /// </summary>
    public uint PlayerId { get; private set; }

    /// <summary>
    /// Gets the object id.
    /// </summary>
    public uint ObjectId { get; private set; }

    public ExpBoxInfoPacket(FFPacket packet)
    {
        PlayerId = packet.ReadUInt32();
        ObjectId = packet.ReadUInt32();
    }
}