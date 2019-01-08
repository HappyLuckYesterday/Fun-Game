﻿using Rhisis.World.Game.Components;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Maps.Regions;

namespace Rhisis.World.Game.Entities
{
    public interface IItemEntity : IEntity
    {
        DropComponent Drop { get; set; }

        IMapRespawnRegion Region { get; set; }
    }
}
