using Rhisis.Core.Helpers;
using Rhisis.Game.Common;
using Rhisis.Protocol;
using System.Collections.Generic;

namespace Rhisis.Game.Entities;

public class WorldObject
{
    public uint ObjectId { get; } = FFRandom.GenerateUniqueId();

    public int ModelId { get; init; }

    public short Size { get; set; } = 100;

    public virtual WorldObjectType Type => WorldObjectType.Object;

    public Map Map { get; set; }

    public MapLayer MapLayer { get; set; }

    public Vector3 Position { get; init; }

    public float RotationAngle { get; set; }

    public string Name { get; init; }

    public bool IsSpawned { get; set; }

    public bool IsVisible { get; set; } = true;

    public ObjectState ObjectState { get; set; } 

    public StateFlags ObjectStateFlags { get; set; }

    public StateMode StateMode { get; set; } = StateMode.NONE;

    /// <summary>
    /// Gets or sets a collection of visible objects for the current entity.
    /// </summary>
    public IList<WorldObject> VisibleObjects { get; } = new List<WorldObject>();

    protected WorldObject()
    {
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
}