using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Network.Snapshots
{
    public class AddObjectSnapshot : FFPacket
    {
        public AddObjectSnapshot(SnapshotType snapshot, int objectId)
        {
            Write(objectId);
            Write((short)snapshot);
        }
    }
}
