﻿using Rhisis.Core.Common;
using Rhisis.Core.Structures;
using Rhisis.World.Game.Core;
using Rhisis.World.Game.Maps;
using System.Collections.Generic;

namespace Rhisis.World.Game.Components
{
    public class ObjectComponent
    {
        public const short DefaultObjectSize = 100;

        /// <summary>
        /// Gets or sets the model id.
        /// </summary>
        public int ModelId { get; set; }

        /// <summary>
        /// Gets or sets the world object type.
        /// </summary>
        public WorldObjectType Type { get; set; }

        /// <summary>
        /// Gets or sets the map id.
        /// </summary>
        public int MapId { get; set; }

        /// <summary>
        /// Gets or sets the map layer id.
        /// </summary>
        public int LayerId { get; set; }

        /// <summary>
        /// Gets or sets the object position.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the object orientation angle.
        /// </summary>
        public float Angle { get; set; }

        /// <summary>
        /// Gets or sets the object size.
        /// </summary>
        public short Size { get; set; }

        /// <summary>
        /// Gets or sets the object name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the object spawn state.
        /// </summary>
        public bool Spawned { get; set; }

        /// <summary>
        /// Gets or sets the object's level.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets the list of the visible entities around.
        /// </summary>
        public IList<IEntity> Entities { get; }

        /// <summary>
        /// Gets the current map instance.
        /// </summary>
        public IMapInstance CurrentMap => WorldServer.Maps.TryGetValue(this.MapId, out IMapInstance map) ? map : null;

        /// <summary>
        /// Gets the current map layer.
        /// </summary>
        public IMapLayer CurrentLayer => this.CurrentMap?.GetMapLayer(this.LayerId);

        /// <summary>
        /// Creates and initializes a new <see cref="ObjectComponent"/>.
        /// </summary>
        public ObjectComponent()
        {
            this.Position = new Vector3();
            this.Entities = new List<IEntity>();
        }

        /// <summary>
        /// To String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Object: {this.Name}";
        }
    }
}
