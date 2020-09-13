﻿using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Common;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Game.IO.Dyo;
using Rhisis.World.Game.Behaviors;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Entities.Internal;
using Rhisis.World.Game.Maps;
using Rhisis.World.Game.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Game.Factories.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class NpcFactory : INpcFactory
    {
        private readonly IGameResources _gameResources;
        private readonly IBehaviorManager _behaviorManager;
        private readonly IItemFactory _itemFactory;

        /// <summary>
        /// Creates a new <see cref="NpcFactory"/> instance.
        /// </summary>
        /// <param name="gameResources">Game resources.</param>
        /// <param name="behaviorManager">Behavior manager.</param>
        /// <param name="itemFactory">Item Factory.</param>
        public NpcFactory(IGameResources gameResources, IBehaviorManager behaviorManager, IItemFactory itemFactory)
        {
            _gameResources = gameResources;
            _behaviorManager = behaviorManager;
            _itemFactory = itemFactory;
        }

        /// <inheritdoc />
        public INpcEntity CreateNpc(IMapContext mapContext, NpcDyoElement element)
        {
            int npcModelId = element.Index;

            if (!_gameResources.Movers.TryGetValue(npcModelId, out MoverData moverData))
            {
                throw new ArgumentException($"Cannot find mover with id '{npcModelId}' in game resources.", nameof(npcModelId));
            }

            var npc = new NpcEntity
            {
                Object = new ObjectComponent
                {
                    MapId = mapContext.Id,
                    CurrentMap = mapContext as IMapInstance,
                    ModelId = npcModelId,
                    Name = element.CharacterKey,
                    Angle = element.Angle,
                    Position = element.Position.Clone(),
                    Size = (short)(ObjectComponent.DefaultObjectSize * element.Scale.X),
                    Spawned = true,
                    Type = WorldObjectType.Mover,
                    Level = 1
                },
                Data = moverData
            };
            npc.Behavior = _behaviorManager.GetBehavior(BehaviorType.Npc, npc, npc.Object.ModelId);
            npc.Timers.LastSpeakTime = RandomHelper.Random(10, 15);
            npc.Quests = _gameResources.Quests.Values.Where(x => !string.IsNullOrEmpty(x.StartCharacter) && x.StartCharacter.Equals(npc.Object.Name, StringComparison.OrdinalIgnoreCase)).ToList();
            npc.Hand = _itemFactory.CreateItem(11, 0, ElementType.None, 0);

            if (_gameResources.Npcs.TryGetValue(npc.Object.Name, out NpcData npcData))
            {
                npc.NpcData = npcData;
            }

            if (npc.NpcData != null && npc.NpcData.HasShop)
            {
                const int NpcShopItemsPerTab = 100;
                ShopData npcShopData = npc.NpcData.Shop;
                npc.Shop = new NpcShopItemContainerComponent[npcShopData.Items.Length];

                for (var i = 0; i < npcShopData.Items.Length; i++)
                {
                    npc.Shop[i] = new NpcShopItemContainerComponent(NpcShopItemsPerTab);

                    IEnumerable<Item> shopTabItems = npcShopData.Items[i].Select((item, index) =>
                    {
                        Item shopItem = _itemFactory.CreateItem(item.Id, item.Refine, item.Element, item.ElementRefine);

                        shopItem.Slot = index;
                        shopItem.Quantity = shopItem.Data.PackMax;

                        return shopItem;
                    });

                    npc.Shop[i].Initialize(shopTabItems.Take(NpcShopItemsPerTab));
                }
            }

            return npc;
        }
    }
}
