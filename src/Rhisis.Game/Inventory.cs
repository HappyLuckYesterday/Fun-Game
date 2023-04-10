using Rhisis.Game.Entities;
using System.Collections.Generic;

namespace Rhisis.Game;

public sealed class Inventory : ItemContainer
{
    public const int InventorySize = 42;
    public const int InventoryEquipParts = 31;
    public const int EquipOffset = InventorySize;

    private readonly Player _owner;
    private readonly Dictionary<CoolTimeType, long> _itemsCoolTimes = new()
    {
        { CoolTimeType.None, 0 },
        { CoolTimeType.Food, 0 },
        { CoolTimeType.Pills, 0 },
        { CoolTimeType.Skill, 0 }
    };

    public Inventory(Player owner)
        : base(InventorySize, InventoryEquipParts)
    {
        _owner = owner;
    }
}
