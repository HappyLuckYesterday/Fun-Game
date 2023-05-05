using Rhisis.Core.Extensions;
using Rhisis.Core.IO;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Game.Resources;
using Rhisis.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game;

public sealed class Inventory : ItemContainer
{
    public static readonly int InventorySize = 42;
    public static readonly int InventoryEquipParts = 31;
    public static readonly int EquipOffset = InventorySize;

    private static readonly Item Hand = new(GameResources.Current.Items.Get(DefineItem.II_WEA_HAN_HAND));

    private readonly Player _owner;
    private readonly Dictionary<CoolTimeType, long> _itemsCoolTimes = new()
    {
        { CoolTimeType.None, 0 },
        { CoolTimeType.Food, 0 },
        { CoolTimeType.Pills, 0 },
        { CoolTimeType.Skill, 0 }
    };

    /// <summary>
    /// Creates a new <see cref="Inventory"/> instance.
    /// </summary>
    /// <param name="owner">Player owner.</param>
    public Inventory(Player owner)
        : base(InventorySize, InventoryEquipParts)
    {
        _owner = owner;
    }

    /// <summary>
    /// Moves an item within the player's inventory.
    /// </summary>
    /// <param name="sourceSlot">Source slot.</param>
    /// <param name="destinationSlot">Destination slot.</param>
    /// <exception cref="InvalidOperationException">Source slot or destination slot are not valid.</exception>
    public void MoveItem(int sourceSlot, int destinationSlot)
    {
        if (sourceSlot < 0 || sourceSlot >= MaxCapacity)
        {
            throw new InvalidOperationException("Source slot is out of inventory range.");
        }

        if (destinationSlot < 0 || destinationSlot >= MaxCapacity)
        {
            throw new InvalidOperationException("Destination slot is out of inventory range.");
        }

        if (sourceSlot == destinationSlot)
        {
            return;
        }

        ItemContainerSlot source = GetAtSlot(sourceSlot) ?? throw new InvalidOperationException("Source slot not found.");
        ItemContainerSlot destination = GetAtSlot(destinationSlot);

        if (source.HasItem && destination.HasItem && source.Item.Id == destination.Item.Id && source.Item.Properties.IsStackable)
        {
            // TODO: stack items
        }
        else
        {
            SwapItem(sourceSlot, destinationSlot);

            using MoveItemSnapshot moveItemSnapshot = new(_owner, sourceSlot, destinationSlot);
            _owner.Send(moveItemSnapshot);
        }
    }

    public int CreateItem(Item item, bool sendToPlayer = true)
    {
        IEnumerable<ItemCreationResult> creationResult = base.CreateItem(item);

        if (creationResult.Any())
        {
            if (sendToPlayer)
            {
                using var snapshot = new FFSnapshot();

                foreach (ItemCreationResult itemResult in creationResult)
                {
                    if (itemResult.ActionType == ItemCreationActionType.Add)
                    {
                        snapshot.Merge(new CreateItemSnapshot(_owner, itemResult.Item, itemResult.Index));
                    }
                    else if (itemResult.ActionType == ItemCreationActionType.Update)
                    {
                        snapshot.Merge(new UpdateItemSnapshot(_owner, UpdateItemType.UI_NUM, itemResult.Index, itemResult.Item.Quantity));
                    }
                }

                _owner.Send(snapshot);
            }
        }
        else
        {
            _owner.SendDefinedText(DefineText.TID_GAME_LACKSPACE);
        }

        return creationResult.Sum(x => x.Item.Quantity);
    }

    /// <summary>
    /// Deletes an given quantity from an item container slot.
    /// </summary>
    /// <param name="itemSlot">Item slot.</param>
    /// <param name="quantity">Quantity to delete.</param>
    /// <param name="updateType">Item update type.</param>
    /// <param name="sendToPlayer">Boolean value that indicates if the player should be notified.</param>
    /// <returns>Deleted item quantity.</returns>
    public int DeleteItem(ItemContainerSlot itemSlot, int quantity, UpdateItemType updateType = UpdateItemType.UI_NUM, bool sendToPlayer = true)
    {
        int quantityToDelete = Math.Min(itemSlot.Item.Quantity, quantity);

        itemSlot.Item.Quantity -= quantityToDelete;

        if (sendToPlayer)
        {
            using UpdateItemSnapshot snapshot = new(_owner, updateType, itemSlot.Index, itemSlot.Item.Quantity);

            _owner.Send(snapshot);
        }

        if (itemSlot.Item.Quantity <= 0)
        {
            if (itemSlot.Number > EquipOffset)
            {
                UnequipSlot(itemSlot);
            }

            Remove(itemSlot);
        }

        return quantityToDelete;
    }

    /// <summary>
    /// Gets the equiped item from the given item part.
    /// </summary>
    /// <param name="equipedItemPart">Item part.</param>
    /// <returns>The item if found; hand otherwise.</returns>
    public Item GetEquipedItem(ItemPartType equipedItemPart)
    {
        ItemContainerSlot equipedItemSlot = GetEquipedItemSlot(equipedItemPart);

        return equipedItemSlot.HasItem ? equipedItemSlot.Item : Hand;
    }

    /// <summary>
    /// Equip an item.
    /// </summary>
    /// <param name="item">Item to equip.</param>
    /// <returns>True if equiped; false otherwise.</returns>
    public bool Equip(ItemContainerSlot slot)
    {
        if (!slot.HasItem)
        {
            return false;
        }

        if (!IsItemEquipable(slot.Item))
        {
            return false;
        }

        if (EquipInternal(slot))
        {
            using DoEquipSnapshot equipSnapshot = new(_owner, slot.Item, slot.Index, true);
            _owner.Send(equipSnapshot);
        }

        return false;
    }

    private bool EquipInternal(ItemContainerSlot slot)
    {
        ItemContainerSlot equipedItem = GetEquipedItemSlot(slot.Item.Properties.Parts);

        if (equipedItem is not null && equipedItem.HasItem)
        {
            UnequipInternal(equipedItem);
        }

        int sourceSlot = slot.Number;
        int destinationSlot = Capacity + (int)slot.Item.Properties.Parts;

        if (slot.Number == destinationSlot || slot.Number >= MaxCapacity || destinationSlot >= MaxCapacity)
        {
            return false;
        }

        for (int i = 0; i < MaxCapacity; i++)
        {
            if (!_items[i].HasItem && _items[i].Number == -1)
            {
                _slots[destinationSlot] = _slots[sourceSlot];
                _slots[sourceSlot] = i;

                _items[_slots[sourceSlot]].Number = sourceSlot;
                _items[_slots[destinationSlot]].Number = destinationSlot;

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Unequip an item;
    /// </summary>
    /// <param name="item">Item to unequip.</param>
    /// <returns>True if unequiped; false otherwise.</returns>
    public bool Unequip(ItemContainerSlot slot)
    {
        if (!slot.HasItem)
        {
            return false;
        }

        if (UnequipInternal(slot))
        {
            using DoEquipSnapshot equipSnapshot = new(_owner, slot.Item, slot.Index, false);
            _owner.Send(equipSnapshot);

            return true;
        }

        return false;
    }

    private bool UnequipInternal(ItemContainerSlot slot)
    {
        if (slot.Number >= MaxCapacity)
        {
            return false;
        }

        int itemIndex = _slots[slot.Number];

        if (itemIndex >= MaxCapacity)
        {
            return false;
        }

        for (int i = 0; i < Capacity; i++)
        {
            // Find empty slot
            int emptyItemIndex = _slots[i];

            if (emptyItemIndex < 0 || emptyItemIndex >= MaxCapacity)
            {
                return false;
            }

            if (!_items[emptyItemIndex].HasItem)
            {
                _items[emptyItemIndex].Number = -1;
                _slots[slot.Number] = -1;

                _items[itemIndex].Number = i;
                _slots[i] = itemIndex;

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if the given item can be equiped based on the player's information.
    /// </summary>
    /// <param name="item">Item to equip.</param>
    /// <returns>True if the item can be equiped; false otherwise.</returns>
    public bool IsItemEquipable(Item item)
    {
        if (item.Properties.ItemSex != int.MaxValue && (GenderType)item.Properties.ItemSex != _owner.Appearence.Gender)
        {
            _owner.SendDefinedText(DefineText.TID_GAME_WRONGSEX, item.Name);

            return false;
        }

        if (_owner.Level < item.Properties.LimitLevel)
        {
            _owner.SendDefinedText(DefineText.TID_GAME_REQLEVEL, item.Properties.LimitLevel.ToString());

            return false;
        }

        if (!_owner.Job.IsAnteriorJob(item.Properties.ItemJob))
        {
            _owner.SendDefinedText(DefineText.TID_GAME_WRONGJOB);

            return false;
        }

        Item equipedItem = GetEquipedItem(ItemPartType.RightWeapon);

        if (item.Properties.ItemKind3 == ItemKind3.ARROW && (equipedItem == null || equipedItem.Properties.ItemKind3 != ItemKind3.BOW))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Use an item.
    /// </summary>
    /// <param name="item">Item to use.</param>
    /// <exception cref="InvalidOperationException">Thrown when the item has not been found.</exception>
    /// <exception cref="NotImplementedException"></exception>
    public void UseItem(Item item)
    {
        ItemContainerSlot itemSlot = _items.FirstOrDefault(x => x.HasItem && x.Item.Id == item.Id && x.Item.SerialNumber == item.SerialNumber);

        if (itemSlot is null)
        {
            throw new InvalidOperationException("Failed to find item in inventory.");
        }

        if (item.Properties.IsUseable && item.Quantity > 0)
        {
            if (ItemHasCoolTime(item) && !CanUseItemWithCoolTime(item))
            {
                return;
            }

            // TODO: implement custom item usage
            // TODO: check for custom items usages

            switch (item.Properties.ItemKind2)
            {
                case ItemKind2.POTION:
                case ItemKind2.REFRESHER:
                case ItemKind2.FOOD:
                    //_inventoryItemUsage.Value.UseFoodItem(_player, item);
                    break;
                case ItemKind2.BLINKWING:
                    //_inventoryItemUsage.Value.UseBlinkwingItem(_player, item);
                    break;
                case ItemKind2.MAGIC:
                    //_inventoryItemUsage.Value.UseMagicItem(_player, item);
                    break;
                default: throw new NotImplementedException($"Item usage {item.Properties.ItemKind2} is not implemented.");
            }
        }
    }

    /// <summary>
    /// Checks if the item can be used.
    /// </summary>
    /// <param name="item">Item to use.</param>
    /// <returns>True if the item can be used; false otherwise.</returns>
    public bool CanUseItemWithCoolTime(Item item)
    {
        CoolTimeType group = GetItemCoolTimeGroup(item);

        return group != CoolTimeType.None && _itemsCoolTimes[group] < Time.GetElapsedTime();
    }

    /// <summary>
    /// Sets a cool time for the given item.
    /// </summary>
    /// <param name="item">Item.</param>
    /// <param name="cooltime">Cool time</param>
    public void SetCoolTime(Item item, int cooltime)
    {
        CoolTimeType group = GetItemCoolTimeGroup(item);

        if (group != CoolTimeType.None)
        {
            _itemsCoolTimes[group] = Time.GetElapsedTime() + cooltime;
        }
    }

    /// <summary>
    /// Check if the item has a cool down.
    /// </summary>
    /// <param name="item">Item.</param>
    /// <returns>True if the item has a cool down time; false otherwise.</returns>
    private static bool ItemHasCoolTime(Item item) => GetItemCoolTimeGroup(item) != CoolTimeType.None;

    /// <summary>
    /// Gets the item cool time group.
    /// </summary>
    /// <param name="item">Item.</param>
    /// <returns>Returns the item cool time group.</returns>
    private static CoolTimeType GetItemCoolTimeGroup(Item item)
    {
        if (item.Properties.CoolTime <= 0)
        {
            return CoolTimeType.None;
        }

        return item.Properties.ItemKind2 switch
        {
            ItemKind2.FOOD => item.Properties.ItemKind3 == ItemKind3.PILL ? CoolTimeType.Pills : CoolTimeType.Food,
            ItemKind2.SKILL => CoolTimeType.Skill,
            _ => CoolTimeType.None,
        };
    }

    private bool UnequipSlot(ItemContainerSlot itemSlot)
    {
        return false;
    }

    private ItemContainerSlot GetEquipedItemSlot(ItemPartType equipedItemPart)
    {
        int equipedItemSlot = EquipOffset + (int)equipedItemPart;

        if (equipedItemSlot > MaxCapacity || equipedItemSlot < EquipOffset)
        {
            return ItemContainerSlot.Empty;
        }

        return GetAtSlot(equipedItemSlot);
    }
}
