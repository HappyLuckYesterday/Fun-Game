using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Network.Snapshots;
using System;
using System.Collections.Generic;

namespace Rhisis.Game.Systems
{
    [Injectable(ServiceLifetime.Singleton)]
    public class InventoryItemUsageSystem : GameFeature, IInventoryItemUsage
    {
        private readonly ISpecialEffectSystem _specialEffectSystem;
        private readonly IMapManager _mapManager;

        public InventoryItemUsageSystem(ISpecialEffectSystem specialEffectSystem, IMapManager mapManager)
        {
            _specialEffectSystem = specialEffectSystem;
            _mapManager = mapManager;
        }

        public void UseBlinkwingItem(IPlayer player, IItem blinkwing)
        {
            if (player.Level < blinkwing.Data.LimitLevel)
            {
                SendDefinedText(player, DefineText.TID_GAME_USINGNOTLEVEL);
                return;
            }

            // TODO: Check if player is sit
            // TODO: Check if player is on Kebaras island
            // TODO: Check if player is in guild war map

            int teleportMapId = player.Map.Id;
            Vector3 destinationPosition;

            if (blinkwing.Data.ItemKind3 == ItemKind3.TOWNBLINKWING)
            {
                IMapRevivalRegion revivalRegion = player.Map.GetNearRevivalRegion(player.Position);

                if (revivalRegion == null)
                {
                    throw new InvalidOperationException($"Cannot find any revival region for map '{player.Map.Name}'.");
                }

                if (player.Map.Id != revivalRegion.MapId)
                {
                    IMap revivalMap = _mapManager.GetMap(revivalRegion.MapId);

                    if (revivalMap == null)
                    {
                        throw new InvalidOperationException($"Cannot find revival map with id '{revivalRegion.MapId}'.");
                    }

                    revivalRegion = revivalMap.GetRevivalRegion(revivalRegion.Key);
                }

                teleportMapId = revivalRegion.MapId;
                destinationPosition = revivalRegion.RevivalPosition.Clone();
            }
            else
            {
                teleportMapId = blinkwing.Data.WeaponTypeId;
                destinationPosition = new Vector3(
                    blinkwing.Data.ItemAtkOrder1, 
                    blinkwing.Data.ItemAtkOrder2,
                    blinkwing.Data.ItemAtkOrder3);
            }

            player.Inventory.ItemInUseActionId = player.Delayer.DelayAction(TimeSpan.FromMilliseconds(blinkwing.Data.SkillReadyType), () =>
            {
                player.Angle = blinkwing.Data.ItemAtkOrder4;
                player.Teleport(destinationPosition, teleportMapId);
                _specialEffectSystem.SetStateModeBaseMotion(player, StateModeBaseMotion.BASEMOTION_OFF);
                player.Inventory.ItemInUseActionId = Guid.Empty;
                DecreaseItem(player, blinkwing);
            });

            _specialEffectSystem.SetStateModeBaseMotion(player, StateModeBaseMotion.BASEMOTION_ON, blinkwing);
        }

        public void UseFoodItem(IPlayer player, IItem foodItem)
        {
            bool itemUsed = false;

            foreach (KeyValuePair<DefineAttributes, int> parameter in foodItem.Data.Params)
            {
                if (parameter.Key == DefineAttributes.HP || parameter.Key == DefineAttributes.MP || parameter.Key == DefineAttributes.FP)
                {
                    int itemMaxRecovery = foodItem.Data.AbilityMin;
                    int currentPoints = player.Health.GetCurrent(parameter.Key);
                    int maxPoints = player.Health.GetMaximum(parameter.Key);

                    if (currentPoints < 0 || maxPoints < 0)
                    {
                        continue;
                    }

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
                                    case DefineAttributes.HP: SendDefinedText(player, DefineText.TID_GAME_LIMITHP); break;
                                    case DefineAttributes.MP: SendDefinedText(player, DefineText.TID_GAME_LIMITMP); break;
                                    case DefineAttributes.FP: SendDefinedText(player, DefineText.TID_GAME_LIMITFP); break;
                                }

                                currentPoints += (int)limitedRecovery;
                            }
                        }
                        else
                        {
                            currentPoints = Math.Min(currentPoints + parameter.Value, maxPoints);
                        }
                    }

                    player.Health.SetCurrent(parameter.Key, currentPoints);

                    itemUsed = true;
                }
            }

            if (itemUsed)
            {
                DecreaseItem(player, foodItem);
            }
        }

        public void UseMagicItem(IPlayer player, IItem magicItem)
        {
            throw new NotImplementedException();
        }

        public void UsePerin(IPlayer player, IItem perinItem)
        {
            throw new NotImplementedException();
        }

        private void DecreaseItem(IPlayer player, IItem item, bool followObject = true)
        {
            var itemUpdateType = UpdateItemType.UI_NUM;

            if (player.Inventory.ItemHasCoolTime(item))
            {
                itemUpdateType = UpdateItemType.UI_COOLTIME;
                player.Inventory.SetCoolTime(item, item.Data.CoolTime);
            }

            if (!item.Data.IsPermanant)
            {
                player.Inventory.DeleteItem(item, 1, updateType: itemUpdateType);
            }

            if (item.Data.SfxObject3 != 0)
            {
                _specialEffectSystem.StartSpecialEffect(player, (DefineSpecialEffects)item.Data.SfxObject3, followObject);
            }
        }
    }
}
