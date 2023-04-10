using Rhisis.Protocol;

namespace Rhisis.Game.Entities;

public sealed class MapItemObject : WorldObject
{
    public void Serialize(FFPacket packet)
    {
        packet.WriteInt32(-1);
        // TODO: serialize item
    }
}