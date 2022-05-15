using Rhisis.Abstractions.Protocol;
using Rhisis.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Protocol.Packets
{
    public class JoinCompletePacket : FFPacket
    {
        public JoinCompletePacket()
            : base(PacketType.JOIN)
        {
        }

        public void AddSnapshots(IEnumerable<IFFSnapshot> snapshots)
        {
            short snapshotCount = (short)snapshots.Sum(x => x.Count);

            WriteInt32(0); // Not used.
            WriteInt16(snapshotCount); // Snapshot amount.

            foreach (IFFSnapshot snapshot in snapshots)
            {
                byte[] snapshotData = snapshot.GetSnapshotContent();

                Write(snapshotData, 0, snapshotData.Length);

                snapshot.Dispose();
            }
        }

        public void AddSnapshots(params IFFSnapshot[] snapshots) => AddSnapshots(snapshots.AsEnumerable());
    }
}
