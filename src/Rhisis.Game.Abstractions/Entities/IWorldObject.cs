using Rhisis.Core.Common;
using Rhisis.Core.Data;
using Rhisis.Core.Structures;
using System;

namespace Rhisis.Game.Abstractions.Entities
{
    public interface IWorldObject
    {
        uint Id { get; }

        WorldObjectType Type { get; }

        int ModelId { get; }

        IMap Map { get; }

        IMapLayer MapLayer { get; }

        Vector3 Position { get; }

        float Angle { get; }

        float Size { get; }

        string Name { get; }

        IServiceProvider Systems { get; }

        bool Spawned { get; set; }

        /// <summary>
        /// Gets or sets the current object state.
        /// </summary>
        /// <remarks>
        /// This property holds the "moving flag" values.
        /// </remarks>
        ObjectState ObjectState { get; set; }

        /// <summary>
        /// Gets or sets the current object state flags.
        /// </summary>
        /// <remarks>
        /// This property holds the "motion flags" values.
        /// </remarks>
        StateFlags ObjectStateFlags { get; set; }
    }
}
