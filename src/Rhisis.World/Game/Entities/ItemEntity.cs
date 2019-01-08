﻿using Rhisis.Core.Common;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Maps.Regions;

namespace Rhisis.World.Game.Entities
{
    public class ItemEntity : Entity, IItemEntity
    {
        public DropComponent Drop { get; set; }

        public override WorldEntityType Type => WorldEntityType.Drop;

        public IMapRespawnRegion Region { get; set; }

        public ItemEntity(IContext context) 
            : base(context)
        {
            this.Object.Type = WorldObjectType.Item;
            this.Drop = new DropComponent();
        }
    }
}
