﻿using Bogus;
using Rhisis.Database.Entities;

namespace Rhisis.World.Tests.Mocks.Database.Entities
{
    internal class ItemEntityGenerator : Faker<DbItem>
    {
        public ItemEntityGenerator(int playerId)
        {
            RuleFor(x => x.ItemId, (f, i) => f.Random.UShort())
                .RuleFor(x => x.ItemSlot, (f, i) => f.IndexFaker)
                .RuleFor(x => x.ItemCount, (f, i) => f.Random.Int(0, 999))
                .RuleFor(x => x.IsDeleted, (f, i) => f.Random.Int() % 5 == 0)
                .RuleFor(x => x.Refine, (f, i) => f.Random.Byte(0, 10))
                .RuleFor(x => x.Element, (f, i) => f.Random.Byte(0, 5))
                .RuleFor(x => x.ElementRefine, (f, i) => f.Random.Byte(0, 10))
                .RuleFor(x => x.CharacterId, playerId);
        }
    }
}
