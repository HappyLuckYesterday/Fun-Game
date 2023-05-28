using Rhisis.Core.Helpers;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Entities;

public class WorldObject : IDisposable
{
    /// <summary>
    /// Gets the object's id.
    /// </summary>
    public uint ObjectId { get; } = FFRandom.GenerateUniqueId();

    /// <summary>
    /// Gets the object model id.
    /// </summary>
    public int ModelId { get; init; }

    /// <summary>
    /// Gets or sets the object size.
    /// </summary>
    public short Size { get; set; } = 100;

    /// <summary>
    /// Gets the object type.
    /// </summary>
    public virtual WorldObjectType Type => WorldObjectType.Object;

    /// <summary>
    /// Gets or sets the current object's map.
    /// </summary>
    public Map Map { get; set; }

    /// <summary>
    /// Gets or sets the current object's map layer.
    /// </summary>
    public MapLayer MapLayer { get; set; }

    /// <summary>
    /// Gets the current object's position.
    /// </summary>
    public Vector3 Position { get; init; }

    /// <summary>
    /// Gets the current object's rotation angle.
    /// </summary>
    public float RotationAngle { get; set; }

    /// <summary>
    /// Gets the current object's name.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets or sets a boolean value that indicates if the object is spawned.
    /// </summary>
    public bool IsSpawned { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates if the object is visible from other objects.
    /// </summary>
    public bool IsVisible { get; set; } = true;
    
    /// <summary>
    /// Gets or sets the object state.
    /// </summary>
    public ObjectState ObjectState { get; set; } 

    /// <summary>
    /// Gets or sets the object state flags.
    /// </summary>
    public StateFlags ObjectStateFlags { get; set; }

    /// <summary>
    /// Gets or sets the object state mode.
    /// </summary>
    public StateMode StateMode { get; set; } = StateMode.NONE;

    /// <summary>
    /// Gets or sets a collection of visible objects for the current entity.
    /// </summary>
    public IList<WorldObject> VisibleObjects { get; } = new List<WorldObject>();

    protected WorldObject()
    {
    }

    /// <summary>
    /// Gets the object identified by the given id that is visible from the current object.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <param name="objectId">Object id to search for.</param>
    /// <returns>World object entity if found; null otherwise.</returns>
    public TEntity GetVisibleObject<TEntity>(uint objectId) where TEntity : WorldObject 
        => VisibleObjects.OfType<TEntity>().SingleOrDefault(x => x.ObjectId == objectId);

    /// <summary>
    /// Sends a defined text to the current world object.
    /// </summary>
    /// <param name="text">Defined text.</param>
    /// <param name="parameters">Text parameters.</param>
    public void SendDefinedText(DefineText text, params object[] parameters)
    {
        using DefinedTextSnapshot snapshot = new(this, text, parameters);

        Send(snapshot);
    }

    /// <summary>
    /// Sends a special effect to the current world object.
    /// </summary>
    /// <param name="specialEffect">Special effect.</param>
    /// <param name="followObject">Boolean value that indicates if the effect should follow the object.</param>
    public void SendSpecialEffect(DefineSpecialEffects specialEffect, bool followObject = true)
    {
        using CreateSfxObjectSnapshot snapshot = new(this, specialEffect, followObject);

        SendToVisible(snapshot, sendToSelf: true);
    }

    /// <summary>
    /// Sends a packet to the current object.
    /// </summary>
    /// <param name="packet"></param>
    public virtual void Send(FFPacket packet)
    {
    }

    /// <summary>
    /// Sends a packet to every visible objects.
    /// </summary>
    /// <param name="packet"></param>
    /// <param name="sendToSelf">When set to true, sends the packet to the current object.</param>
    public virtual void SendToVisible(FFPacket packet, bool sendToSelf = false)
    {
        foreach (WorldObject @object in VisibleObjects)
        {
            @object.Send(packet);
        }

        if (sendToSelf)
        {
            Send(packet);
        }
    }

    /// <summary>
    /// Disposes the current object.
    /// </summary>
    public virtual void Dispose()
    {
    }
}