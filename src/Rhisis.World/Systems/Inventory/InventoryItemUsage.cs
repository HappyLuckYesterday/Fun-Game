﻿using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Helpers;
using Rhisis.World.Game.Maps;
using Rhisis.World.Game.Maps.Regions;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.SpecialEffect;
using Rhisis.World.Systems.Teleport;
using System;
using System.Collections.Generic;

namespace Rhisis.World.Systems.Inventory
{
    [Injectable]
    public sealed class InventoryItemUsage : IInventoryItemUsage
    {
        private readonly ILogger<InventoryItemUsage> _logger;
        private readonly IInventoryPacketFactory _inventoryPacketFactory;
        private readonly IMapManager _mapManager;
        private readonly ISpecialEffectSystem _specialEffectSystem;
        private readonly ITeleportSystem _teleportSystem;
        private readonly IMoverPacketFactory _moverPacketFactory;
        private readonly ITextPacketFactory _textPacketFactory;

        /// <summary>
        /// Creates a new <see cref="InventoryItemUsage"/> instance.
        /// </summary>
        public InventoryItemUsage(ILogger<InventoryItemUsage> logger, IInventoryPacketFactory inventoryPacketFactory, IMapManager mapManager, ISpecialEffectSystem specialEffectSystem, ITeleportSystem teleportSystem, IMoverPacketFactory moverPacketFactory, ITextPacketFactory textPacketFactory)
        {
            this._logger = logger;
            this._inventoryPacketFactory = inventoryPacketFactory;
            this._mapManager = mapManager;
            this._specialEffectSystem = specialEffectSystem;
            this._teleportSystem = teleportSystem;
            this._moverPacketFactory = moverPacketFactory;
            this._textPacketFactory = textPacketFactory;
        }

        public void UseFoodItem(IPlayerEntity player, Item foodItemToUse)
        {
            foreach (KeyValuePair<DefineAttributes, int> parameter in foodItemToUse.Data.Params)
            {
                if (parameter.Key == DefineAttributes.HP || parameter.Key == DefineAttributes.MP || parameter.Key == DefineAttributes.FP)
                {
                    int currentPoints = PlayerHelper.GetPoints(player, parameter.Key);
                    int maxPoints = PlayerHelper.GetMaxPoints(player, parameter.Key);
                    int itemMaxRecovery = foodItemToUse.Data.AbilityMin;

                    if (parameter.Value >= 0)
                    {
                        if (currentPoints >= itemMaxRecovery)
                        {
                            float limitedRecovery = parameter.Value * 0.3f;

                            if (currentPoints + limitedRecovery > maxPoints)
                            {
                                currentPoints = maxPoints;
                            }
                            else
                            {
                                switch (parameter.Key)
                                {
                                    case DefineAttributes.HP: this._textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LIMITHP); break;
                                    case DefineAttributes.MP: this._textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LIMITMP); break;
                                    case DefineAttributes.FP: this._textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_LIMITFP); break;
                                }

                                currentPoints += (int)limitedRecovery;
                            }
                        }
                        else
                        {
                            currentPoints = Math.Min(currentPoints + parameter.Value, maxPoints);
                        }
                    }

                    PlayerHelper.SetPoints(player, parameter.Key, currentPoints);
                    this._moverPacketFactory.SendUpdateAttributes(player, parameter.Key, PlayerHelper.GetPoints(player, parameter.Key));
                }
                else
                {
                    // TODO: food triggers a skill.
                    this._logger.LogWarning($"Activating a skill throught food.");
                    this._textPacketFactory.SendFeatureNotImplemented(player, "skill with food");
                }
            }

            this.DecreaseItem(player, foodItemToUse);
        }

        public void UseBlinkwingItem(IPlayerEntity player, Item blinkwing)
        {
            if (player.Object.Level < blinkwing.Data.LimitLevel)
            {
                this._logger.LogError($"Player {player.Object.Name} cannot use {blinkwing.Data.Name}. Level too low.");
                this._textPacketFactory.SendDefinedText(player, DefineText.TID_GAME_USINGNOTLEVEL);
                return;
            }

            // TODO: Check if player is sit
            // TODO: Check if player is on Kebaras island
            // TODO: Check if player is in guild war map

            TeleportEventArgs teleportEvent;

            if (blinkwing.Data.ItemKind3 == ItemKind3.TOWNBLINKWING)
            {
                IMapRevivalRegion revivalRegion = player.Object.CurrentMap.GetNearRevivalRegion(player.Object.Position);

                if (revivalRegion == null)
                {
                    this._logger.LogError($"Cannot find any revival region for map '{player.Object.CurrentMap.Name}'.");
                    return;
                }
                if (player.Object.MapId != revivalRegion.MapId)
                {
                    IMapInstance revivalMap = this._mapManager.GetMap(revivalRegion.MapId);

                    if (revivalMap == null)
                    {
                        this._logger.LogError($"Cannot find revival map with id '{revivalRegion.MapId}'.");
                        // TODO: disconnect client
                        //player.Connection.Server.DisconnectClient(player.Connection.Id);
                        return;
                    }

                    revivalRegion = revivalMap.GetRevivalRegion(revivalRegion.Key);
                }

                teleportEvent = new TeleportEventArgs(revivalRegion.MapId,
                    revivalRegion.RevivalPosition.X,
                    revivalRegion.RevivalPosition.Z,
                    revivalRegion.RevivalPosition.Y);
            }
            else
            {
                teleportEvent = new TeleportEventArgs(blinkwing.Data.WeaponTypeId, // Map Id
                    blinkwing.Data.ItemAtkOrder1, // X
                    blinkwing.Data.ItemAtkOrder3, // Z
                    blinkwing.Data.ItemAtkOrder2, // Y
                    blinkwing.Data.ItemAtkOrder4); // Angle
            }

            player.Inventory.ItemInUseActionId = player.Delayer.DelayAction(TimeSpan.FromMilliseconds(blinkwing.Data.SkillReadyType), () =>
            {
                this._teleportSystem.Teleport(player, teleportEvent.MapId, teleportEvent.PositionX, teleportEvent.PositionY, teleportEvent.PositionZ, teleportEvent.Angle);
                this._specialEffectSystem.SetStateModeBaseMotion(player, StateModeBaseMotion.BASEMOTION_OFF);
                player.Inventory.ItemInUseActionId = Guid.Empty;
                this.DecreaseItem(player, blinkwing);
            });

            this._specialEffectSystem.SetStateModeBaseMotion(player, StateModeBaseMotion.BASEMOTION_ON, blinkwing);
        }

        public void UseMagicItem(IPlayerEntity player, Item magicItem)
        {
            bool noFollowSfx = true;
            player.Inventory.ItemInUseActionId = player.Delayer.DelayAction(TimeSpan.FromMilliseconds(magicItem.Data.SkillReadyType), () =>
            {
                this._specialEffectSystem.SetStateModeBaseMotion(player, StateModeBaseMotion.BASEMOTION_OFF);
                player.Inventory.ItemInUseActionId = Guid.Empty;
                this.DecreaseItem(player, magicItem, noFollowSfx);
            });
            this._specialEffectSystem.SetStateModeBaseMotion(player, StateModeBaseMotion.BASEMOTION_ON, magicItem);
        }

        /// <summary>
        /// Decreases an item from player's inventory.
        /// </summary>
        /// <param name="player">Player.</param>
        /// <param name="item">Item to decrease.</param>
        private void DecreaseItem(IPlayerEntity player, Item item, bool noFollowSfx = false)
        {
            var itemUpdateType = UpdateItemType.UI_NUM;

            if (player.Inventory.ItemHasCoolTime(item))
            {
                itemUpdateType = UpdateItemType.UI_COOLTIME;
                player.Inventory.SetCoolTime(item, item.Data.CoolTime);
            }

            if (!item.Data.IsPermanant)
                item.Quantity--;


            this._inventoryPacketFactory.SendItemUpdate(player, itemUpdateType, item.UniqueId, item.Quantity);

            if (item.Data.SfxObject3 != 0)
                this._specialEffectSystem.StartSpecialEffect(player, (DefineSpecialEffects)item.Data.SfxObject3, noFollowSfx);

            if (item.Quantity <= 0)
                item.Reset();
        }
    }
}
