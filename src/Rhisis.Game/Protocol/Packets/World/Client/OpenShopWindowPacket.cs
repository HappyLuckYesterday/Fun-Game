using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class OpenShopWindowPacket
{
    /// <summary>
    /// Gets the selected object id.
    /// </summary>
    public uint ObjectId { get; private set; }

    public OpenShopWindowPacket(FFPacket packet)
    {
        ObjectId = packet.ReadUInt32();
    }
}
