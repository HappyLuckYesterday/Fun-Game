using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class FocusObjectPacket
{
    /// <summary>
    /// Gets the object id.
    /// </summary>
    public uint ObjectId { get; private set; }

    public FocusObjectPacket(FFPacket packet)
    {
        ObjectId = packet.ReadUInt32();
    }
}