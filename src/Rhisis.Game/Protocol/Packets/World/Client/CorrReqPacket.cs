using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class CorrReqPacket
{
    /// <summary>
    /// Gets the object id.
    /// </summary>
    public uint ObjectId { get; private set; }

    public CorrReqPacket(FFPacket packet)
    {
        ObjectId = packet.ReadUInt32();
    }
}