using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
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

        public InventoryItemUsageSystem(ISpecialEffectSystem specialEffectSystem)
        {
            _specialEffectSystem = specialEffectSystem;
        }

        public void UseBlinkwingItem(IPlayer player, IItem blinkwing)
        {
            throw new NotImplementedException();
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
