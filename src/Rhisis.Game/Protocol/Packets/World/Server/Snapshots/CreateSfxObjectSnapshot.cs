using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Server.Snapshots;

public class CreateSfxObjectSnapshot : FFSnapshot
{
    public CreateSfxObjectSnapshot(WorldObject worldObject, DefineSpecialEffects specialEffect, bool followObject = true)
        : base(SnapshotType.CREATESFXOBJ, worldObject.ObjectId)
    {
        WriteInt32((int)specialEffect);

        if (followObject)
        {
            WriteSingle(0f);
            WriteSingle(0f);
            WriteSingle(0f);
        }
        else
        {
            WriteSingle(worldObject.Position.X);
            WriteSingle(worldObject.Position.Y);
            WriteSingle(worldObject.Position.Z);
        }
    }
}
