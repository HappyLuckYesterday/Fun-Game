﻿using Rhisis.Core.Data;
using Rhisis.Core.IO;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Inventory;
using Rhisis.World.Systems.Inventory.EventArgs;
using Rhisis.World.Systems.Recovery;
using Rhisis.World.Systems.Recovery.EventArgs;
using System;

namespace Rhisis.World.Game.Behaviors
{
    [Behavior(BehaviorType.Player, IsDefault: true)]
    public sealed class DefaultPlayerBehavior : IBehavior<IPlayerEntity>
    {
        /// <inheritdoc />
        public void Update(IPlayerEntity player)
        {
            this.ProcessIdleHeal(player);
        }

        /// <inheritdoc />
        public void OnArrived(IPlayerEntity player)
        {
            if (player.Follow.IsFollowing && player.Follow.Target.Type == WorldEntityType.Drop)
            {
                this.PickUpDroppedItem(player, player.Follow.Target as IItemEntity);
                player.Follow.Reset();
            }
        }

        /// <summary>
        /// Verify all conditions to pickup a dropped item.
        /// </summary>
        /// <param name="player">The player trying to pick-up the dropped item.</param>
        /// <param name="droppedItem">The dropped item.</param>
        private void PickUpDroppedItem(IPlayerEntity player, IItemEntity droppedItem)
        {
            // TODO: check if drop belongs to a party.

            if (droppedItem.Drop.HasOwner && droppedItem.Drop.Owner != player)
            {
                WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_PRIORITYITEMPER, $"\"{droppedItem.Object.Name}\"");
                return;
            }

            if (droppedItem.Drop.IsGold)
            {
                int droppedGoldAmount = droppedItem.Drop.Item.Quantity;
                long gold = player.PlayerData.Gold + droppedGoldAmount;

                if (gold > int.MaxValue || gold < 0) // Check gold overflow
                {
                    WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_TOOMANYMONEY_USE_PERIN);
                    return;
                }
                else
                {
                    player.PlayerData.Gold = (int)gold;
                    WorldPacketFactory.SendUpdateAttributes(player, DefineAttributes.GOLD, player.PlayerData.Gold);
                    WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_REAPMONEY, droppedGoldAmount.ToString("###,###,###,###"), gold.ToString("###,###,###,###"));
                }
            }
            else
            {
                var inventoryItemCreationEvent = new InventoryCreateItemEventArgs(droppedItem.Drop.Item.Id, droppedItem.Drop.Item.Quantity, -1);
                player.NotifySystem<InventorySystem>(inventoryItemCreationEvent);
                WorldPacketFactory.SendDefinedText(player, DefineText.TID_GAME_REAPITEM, $"\"{droppedItem.Object.Name}\"");
            }

            WorldPacketFactory.SendMotion(player, ObjectMessageType.OBJMSG_PICKUP);
            droppedItem.Delete();
        }

        /// <summary>
        /// Process Idle heal logic when player is not fighting.
        /// </summary>
        /// <param name="player"></param>
        private void ProcessIdleHeal(IPlayerEntity player)
        {
            if (player.Timers.NextHealTime <= Time.TimeInSeconds())
            {
                if (!player.Battle.IsFighting)
                {
                    player.NotifySystem<RecoverySystem>(new IdleRecoveryEventArgs(isSitted: false));
                }
            }
        }
    }
}
