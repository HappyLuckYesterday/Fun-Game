using Rhisis.Game.Common;
using Rhisis.Core.Structures;
using System;
using Rhisis.Game.Abstractions.Map;
using System.Collections.Generic;
using Sylver.Network.Data;

namespace Rhisis.Game.Abstractions.Entities
{
    public interface IWorldObject : IEquatable<IWorldObject>
    {
        uint Id { get; }

        WorldObjectType Type { get; }

        int ModelId { get; }

        IMap Map { get; }

        IMapLayer MapLayer { get; set; }

        Vector3 Position { get; }

        float Angle { get; set; }

        short Size { get; }

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

        /// <summary>
        /// Gets or sets the current object state.
        /// </summary>
        StateMode StateMode { get; set; }

        /// <summary>
        /// Gets or sets a collection of visible objects for the current entity.
        /// </summary>
        IList<IWorldObject> VisibleObjects { get; set; }

        /// <summary>
        /// Sends a packet to the current object.
        /// </summary>
        /// <param name="packet"></param>
        void Send(INetPacketStream packet);

        /// <summary>
        /// Sends a packet to every visible objects.
        /// </summary>
        /// <param name="packet"></param>
        void SendToVisible(INetPacketStream packet);
    }
}
