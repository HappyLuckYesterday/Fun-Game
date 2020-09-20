using Rhisis.Network;
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

        public void AddSnapshots(IEnumerable<FFSnapshot> snapshots)
        {
            short snapshotCount = (short)snapshots.Sum(x => x.Count);

            Write(0); // Not used.
            Write(snapshotCount); // Snapshot amount.

            foreach (FFSnapshot snapshot in snapshots)
            {
                byte[] snapshotData = snapshot.GetContent();

                Write(snapshotData, 0, snapshotData.Length);

                snapshot.Dispose();
            }
        }

        public void AddSnapshots(params FFSnapshot[] snapshots) => AddSnapshots(snapshots.AsEnumerable());
    }
}
