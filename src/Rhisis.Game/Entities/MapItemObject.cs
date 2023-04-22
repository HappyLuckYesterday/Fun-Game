using Rhisis.Game.Common;
using Rhisis.Protocol;

namespace Rhisis.Game.Entities;

public sealed class MapItemObject : WorldObject
{
    public override WorldObjectType Type => WorldObjectType.Item;

    public Item Item { get; }

    public Mover Owner { get; init; }

    public long OwnershipTime { get; init; }

    public bool HasOwner => Owner is not null && OwnershipTime > 0;

    public MapItemType ItemType { get; init; } = MapItemType.DropItem;

    public MapItemObject(Item item)
    {
        Item = item;
        ModelId = item.Properties.Id;
    }

    public void Serialize(FFPacket packet)
    {
        packet.WriteInt32(-1);
        Item.Serialize(packet);
    }
}