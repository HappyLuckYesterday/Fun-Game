using Rhisis.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Snapshots;

public class CreateSfxObjectSnapshot : FFSnapshot
{
    public CreateSfxObjectSnapshot(IWorldObject worldObject, DefineSpecialEffects specialEffect, bool followObject = true)
        : base(SnapshotType.CREATESFXOBJ, worldObject.Id)
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
