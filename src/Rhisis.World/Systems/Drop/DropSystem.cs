﻿using Microsoft.Extensions.Options;
using Rhisis.Core.Data;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.IO;
using Rhisis.Core.Structures;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Factories;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Systems.Drop
{
    [Injectable]
    public sealed class DropSystem : IDropSystem
    {
        public const long MaxDropChance = 3000000000;
        private const int DropGoldLimit1 = 9;
        private const int DropGoldLimit2 = 49;
        private const int DropGoldLimit3 = 99;
        private const float DropCircleRadius = 0.1f;

        private readonly IItemFactory _itemFactory;
        private readonly WorldConfiguration _worldServerConfiguration;

        /// <summary>
        /// Creates a new <see cref="DropSystem"/> instance.
        /// </summary>
        /// <param name="itemFactory">Item factory.</param>
        /// <param name="worldServerConfiguration">World server configuration.</param>
        public DropSystem(IItemFactory itemFactory, IOptions<WorldConfiguration> worldServerConfiguration)
        {
            _itemFactory = itemFactory;
            _worldServerConfiguration = worldServerConfiguration.Value;
        }

        /// <inheritdoc />
        public void DropItem(IWorldEntity entity, int itemId, IWorldEntity owner, int quantity = 1)
        {
            Item itemToDrop = _itemFactory.CreateItem(itemId);

            DropItem(entity, itemToDrop, owner, quantity);
        }

        /// <inheritdoc />
        public void DropItem(IWorldEntity entity, Item item, IWorldEntity owner, int quantity = 1)
        {
            IItemEntity newItem = _itemFactory.CreateItemEntity(entity.Object.CurrentMap, entity.Object.CurrentLayer, item, owner);

            newItem.Drop.Item.Quantity = quantity;
            newItem.Object.Position = Vector3.GetRandomPositionInCircle(entity.Object.Position, DropCircleRadius);

            if (newItem.Drop.Owner != null)
            {
                newItem.Drop.OwnershipTime = Time.TimeInSeconds() + _worldServerConfiguration.Drops.OwnershipTime;
                newItem.Drop.DespawnTime = Time.TimeInSeconds() + _worldServerConfiguration.Drops.DespawnTime;
            }

            entity.Object.CurrentLayer.AddEntity(newItem);
        }

        /// <inheritdoc />
        public void DropGold(IWorldEntity entity, int goldAmount, IWorldEntity owner)
        {
            int goldItemId = DefineItem.II_GOLD_SEED1;
            int gold = goldAmount * _worldServerConfiguration.Rates.Gold;

            if (gold <= 0)
            {
                return;
            }

            if (gold > (DropGoldLimit1 * _worldServerConfiguration.Rates.Gold))
            {
                goldItemId = DefineItem.II_GOLD_SEED2;
            }
            else if (gold > (DropGoldLimit2 * _worldServerConfiguration.Rates.Gold))
            {
                goldItemId = DefineItem.II_GOLD_SEED3;
            }
            else if (gold > (DropGoldLimit3 * _worldServerConfiguration.Rates.Gold))
            {
                goldItemId = DefineItem.II_GOLD_SEED4;
            }

            Item goldItem = _itemFactory.CreateItem(goldItemId, 0, ElementType.None, 0);

            DropItem(entity, goldItem, owner, gold);
        }
    }
}
