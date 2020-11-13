using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Common;
using Sylver.Network.Data;
using System;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Entities
{
    /// <summary>
    /// Describes the world object entity.
    /// </summary>
    /// <remarks>
    /// This is the most common entity of the game. All entities should implement this interface.
    /// </remarks>
    public interface IWorldObject : IEquatable<IWorldObject>
    {
        /// <summary>
        /// Gets the object id.
        /// </summary>
        uint Id { get; }

        /// <summary>
        /// Gets the object type.
        /// </summary>
        WorldObjectType Type { get; }

        /// <summary>
        /// Gets the object model id.
        /// </summary>
        int ModelId { get; }

        /// <summary>
        /// Gets the map where the current object is located.
        /// </summary>
        IMap Map { get; }

        /// <summary>
        /// Gets the map layer where the current object is located.
        /// </summary>
        IMapLayer MapLayer { get; set; }

        /// <summary>
        /// Gets the object position.
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// Gets or sets the object orientation angle.
        /// </summary>
        float Angle { get; set; }

        /// <summary>
        /// Gets the object size.
        /// </summary>
        short Size { get; }

        /// <summary>
        /// Gets the object name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the game systems.
        /// </summary>
        IServiceProvider Systems { get; }

        /// <summary>
        /// Gets or sets a boolean value that indicates if the object is spawned.
        /// </summary>
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
