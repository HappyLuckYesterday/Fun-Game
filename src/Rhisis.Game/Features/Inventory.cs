using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Components;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Snapshots;
using Sylver.Network.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Game.Features
{
    public class Inventory : GameFeature, IInventory
    {
        public const int InventorySize = 42;
        public const int InventoryEquipParts = 31;
        public const int EquipOffset = InventorySize;

        private readonly IPlayer _player;
        private readonly IItemContainer _container;

        public int MaxCapacity => _container.MaxCapacity;

        public Inventory(IPlayer player)
        {
            _player = player;
            _container = new ItemContainerComponent<Item>(InventorySize, InventoryEquipParts);
        }

        public void SetItems(IEnumerable<IItem> items) => _container.Initialize(items);

        public IEnumerable<IItem> GetEquipedItems() => _container.GetRange(InventorySize, InventoryEquipParts);

        public IItem GetItem(Func<IItem, bool> predicate) => _container.GetItem(predicate);

        public IItem GetItem(int itemIndex) => _container.GetItemAtIndex(itemIndex);

        public void Move(int sourceSlot, int destinationSlot)
        {
            if (sourceSlot < 0 || sourceSlot >= _container.MaxCapacity)
            {
                throw new InvalidOperationException("Source slot is out of inventory range.");
            }

            if (destinationSlot < 0 || destinationSlot >= _container.MaxCapacity)
            {
                throw new InvalidOperationException("Destination slot is out of inventory range.");
            }

            if (sourceSlot == destinationSlot)
            {
                throw new InvalidOperationException("Cannot move an item to the same slot.");
            }

            IItem sourceItem = _container.GetItemAtSlot(sourceSlot);

            if (sourceItem == null)
            {
                throw new InvalidOperationException("Source item not found");
            }

            IItem destinationItem = _container.GetItemAtSlot(destinationSlot);

            if (destinationItem != null && sourceItem.Id == destinationItem.Id && sourceItem.Data.IsStackable)
            {
                int newQuantity = sourceItem.Quantity + destinationItem.Quantity;

                using var snapshots = new FFSnapshot();

                if (newQuantity > destinationItem.Data.PackMax)
                {
                    destinationItem.Quantity = destinationItem.Data.PackMax;
                    sourceItem.Quantity = newQuantity - sourceItem.Data.PackMax;

                    snapshots.Merge(new UpdateItemSnapshot(_player, UpdateItemType.UI_NUM, sourceItem.Index, sourceItem.Quantity));
                    snapshots.Merge(new UpdateItemSnapshot(_player, UpdateItemType.UI_NUM, destinationItem.Index, destinationItem.Quantity));
                }
                else
                {
                    destinationItem.Quantity = newQuantity;
                    DeleteItem(sourceItem.Index, sourceItem.Quantity);
                    snapshots.Merge(new UpdateItemSnapshot(_player, UpdateItemType.UI_NUM, destinationItem.Index, destinationItem.Quantity));
                }

                _player.Connection.Send(snapshots);
            }
            else
            {
                _container.SwapItem(sourceSlot, destinationSlot);

                using var moveItemSnapshot = new MoveItemSnapshot(_player, sourceSlot, destinationSlot);
                _player.Connection.Send(moveItemSnapshot);
            }
        }

        public int DeleteItem(int itemIndex, int quantity, bool sendToPlayer = true)
        {
            if (quantity <= 0)
            {
                return 0;
            }

            IItem itemToDelete = _container.GetItemAtIndex(itemIndex);

            if (itemToDelete == null)
            {
                throw new ArgumentException($"Cannot find item with index: '{itemIndex}' in '{_player.Name}''s inventory.", nameof(itemToDelete));
            }

            return DeleteItem(itemToDelete, quantity, sendToPlayer);
        }

        public int DeleteItem(IItem item, int quantity, bool sendToPlayer = true)
        {
            int quantityToDelete = Math.Min(item.Quantity, quantity);

            item.Quantity -= quantityToDelete;

            if (sendToPlayer)
            {
                using var snapshot = new UpdateItemSnapshot(_player, UpdateItemType.UI_NUM, item.Index, item.Quantity);

                _player.Connection.Send(snapshot);
            }

            if (item.Quantity <= 0)
            {
                if (IsItemEquiped(item))
                {
                    InternalUnequip(item);
                }

                _container.DeleteItem(item);
            }

            return quantityToDelete;
        }

        public void Serialize(INetPacketStream packet) => _container.Serialize(packet);

        public IEnumerator<IItem> GetEnumerator() => _container.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _container.GetEnumerator();

        public bool IsItemEquiped(IItem item) => item.Slot > EquipOffset;

        public bool IsItemEquipable(IItem item)
        {
            if (item.Data.ItemSex != int.MaxValue && (GenderType)item.Data.ItemSex != _player.Appearence.Gender)
            {
                using var textSnapshot = new DefinedTextSnapshot(_player, DefineText.TID_GAME_WRONGSEX, item.Name);
                _player.Connection.Send(textSnapshot);

                return false;
            }

            if (_player.Level < item.Data.LimitLevel)
            {
                using var textSnapshot = new DefinedTextSnapshot(_player, DefineText.TID_GAME_REQLEVEL, item.Data.LimitLevel.ToString());
                _player.Connection.Send(textSnapshot);

                return false;
            }

            if (!_player.Job.IsAnteriorJob(item.Data.ItemJob))
            {
                using var textSnapshot = new DefinedTextSnapshot(_player, DefineText.TID_GAME_WRONGJOB);
                _player.Connection.Send(textSnapshot);

                return false;
            }

            IItem equipedItem = GetEquipedItem(ItemPartType.RightWeapon);

            if (item.Data.ItemKind3 == ItemKind3.ARROW && (equipedItem == null || equipedItem.Data.ItemKind3 != ItemKind3.BOW))
            {
                return false;
            }

            return true;
        }

        public IItem GetEquipedItem(ItemPartType equipedItemPart)
        {
            int equipedItemSlot = EquipOffset + (int)equipedItemPart;

            if (equipedItemSlot > _container.MaxCapacity || equipedItemSlot < EquipOffset)
            {
                return null;
            }

            return _container.GetItemAtSlot(equipedItemSlot);
        }

        public bool Equip(IItem itemToEquip)
        {
            if (!IsItemEquipable(itemToEquip))
            {
                return false;
            }

            IItem equipedItem = GetEquipedItem(itemToEquip.Data.Parts);

            if (equipedItem != null)
            {
                InternalUnequip(equipedItem);
            }

            if (InternalEquip(itemToEquip, itemToEquip.Data.Parts))
            {
                using var equipSnapshot = new DoEquipSnapshot(_player, itemToEquip, true);

                _player.Connection.Send(equipSnapshot);
            }

            return true;
        }

        public bool Unequip(IItem itemToUnequip)
        {
            if (InternalUnequip(itemToUnequip))
            {
                using var equipSnapshot = new DoEquipSnapshot(_player, itemToUnequip, true);

                _player.Connection.Send(equipSnapshot);
                return true;
            }

            return false;
        }

        private bool InternalEquip(IItem itemToEquip, ItemPartType destinationPart)
        {
            int sourceSlot = itemToEquip.Slot;
            int destinationSlot = _container.Capacity + (int)destinationPart;

            if (itemToEquip.Slot == destinationSlot || itemToEquip.Slot >= _container.MaxCapacity || destinationSlot >= _container.MaxCapacity)
            {
                return false;
            }

            for (int i = 0; i < _container.MaxCapacity; i++)
            {
                if (_container[i].Id == -1 && _container[i].Slot == -1)
                {
                    _container.Masks[destinationSlot] = _container.Masks[sourceSlot];
                    _container.Masks[sourceSlot] = i;

                    _container[_container.Masks[sourceSlot]].Slot = sourceSlot;
                    _container[_container.Masks[destinationSlot]].Slot = destinationSlot;

                    return true;
                }
            }

            return true;
        }

        private bool InternalUnequip(IItem itemToUnequip)
        {
            int slot = itemToUnequip.Slot;

            if (slot >= _container.MaxCapacity)
            {
                return false;
            }

            int itemIndex = _container.Masks[slot];

            if (itemIndex < 0 || itemIndex >= _container.MaxCapacity)
            {
                return false;
            }

            for (int i = 0; i < _container.Capacity; i++)
            {
                int emptyItemIndex = _container.Masks[i];

                if (emptyItemIndex < 0 || emptyItemIndex >= _container.MaxCapacity)
                {
                    return false;
                }

                if (_container[emptyItemIndex].Id == -1)
                {
                    _container[emptyItemIndex].Slot = -1;
                    _container.Masks[slot] = -1;

                    _container[itemIndex].Slot = i;
                    _container.Masks[i] = itemIndex;

                    return true;
                }
            }

            return false;
        }
    }
}
