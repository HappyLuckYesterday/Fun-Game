using Rhisis.Game.Common;
using Rhisis.Protocol;
using System;

namespace Rhisis.Game.Entities;

public sealed class MapItemObject : WorldObject
{
    public override WorldObjectType Type => WorldObjectType.Item;

    public Item Item { get; }

    public Mover Owner { get; init; }

    public long OwnershipTime { get; init; }

    public bool HasOwner => Owner is not null && OwnershipTime > 0;

    public MapItemType ItemType { get; init; } = MapItemType.DropItem;

    public bool IsGold => Item.Id == DefineItem.II_GOLD_SEED1 ||
        Item?.Id == DefineItem.II_GOLD_SEED2 ||
        Item?.Id == DefineItem.II_GOLD_SEED3 ||
        Item?.Id == DefineItem.II_GOLD_SEED4;

    public MapItemObject(Item item)
    {
        Item = item ?? throw new ArgumentNullException(nameof(item), "Cannot create a map object instance with an undefined item.");
        ModelId = item.Properties.Id;
    }

    public void Serialize(FFPacket packet)
    {
        packet.WriteInt32(-1);
        Item.Serialize(packet);
    }
}