using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Network.Snapshots
{
    public class CreateSfxObjectSnapshot : FFSnapshot
    {
        public CreateSfxObjectSnapshot(IWorldObject worldObject, DefineSpecialEffects specialEffect, bool followObject = true)
            : base(SnapshotType.CREATESFXOBJ, worldObject.Id)
        {
            Write((int)specialEffect);

            if (followObject)
            {
                Write(0f);
                Write(0f);
                Write(0f);
            }
            else
            {
                Write(worldObject.Position.X);
                Write(worldObject.Position.Y);
                Write(worldObject.Position.Z);
            }
        }
    }
}
