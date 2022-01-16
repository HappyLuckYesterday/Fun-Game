using Rhisis.Abstractions.Protocol;
using Rhisis.Core.Extensions;
using System.IO;
using System.Linq;

namespace Rhisis.Protocol
{
    /// <summary>
    /// Represents a FlyFF snapshot packet.
    /// </summary>
    public class FFSnapshot : FFPacket, IFFSnapshot
    {
        /// <summary>
        /// Gets the FlyFF snapshot header offset in the packet stream.
        /// </summary>
        /// <remarks>
        /// The snapshot header is located at offset 5.
        /// </remarks>
        public static readonly int SnapshotHeaderOffset = sizeof(byte) + sizeof(int);

        /// <summary>
        /// Gets the FlyFF snapshot amount value offset in the packet stream.
        /// </summary>
        /// <remarks>
        /// The snapshot amount value is located at offset 13.
        /// </remarks>
        public static readonly int SnapshotAmountOffset = SnapshotHeaderOffset + sizeof(int) + sizeof(int);

        /// <summary>
        /// Gets the FlyFF snapshot content value offset in the packet stream.
        /// </summary>
        public static readonly int SnapshotContentOffset = SnapshotAmountOffset + sizeof(short);

        public short Count { get; private set; } = 0;

        /// <summary>
        /// Creates a new empty <see cref="FFSnapshot"/> instance.
        /// </summary>
        public FFSnapshot()
            : base(PacketType.SNAPSHOT)
        {
            Write(0);
            Write(Count);
        }

        /// <summary>
        /// Creates a new <see cref="FFSnapshot"/> instance and merges the given snapshots into it.
        /// </summary>
        /// <param name="snapshots">Snapshots to merge.</param>
        public FFSnapshot(params IFFSnapshot[] snapshots)
            : this()
        {
            foreach (var snapshot in snapshots)
            {
                Merge(snapshot);
            }
        }

        /// <summary>
        /// Creates a new <see cref="FFSnapshot"/> instance.
        /// </summary>
        /// <param name="snapshot">Snapshot type.</param>
        /// <param name="objectId">Target object id.</param>
        public FFSnapshot(SnapshotType snapshot, uint objectId)
            : base(PacketType.SNAPSHOT)
        {
            Write(0); // Not used.
            Write(++Count); // Snapshot amount.
            Write(objectId);
            Write((short)((uint)snapshot));
        }

        public IFFSnapshot Merge(IFFSnapshot snapshot)
        {
            Count += snapshot.Count;

            Seek(SnapshotAmountOffset, SeekOrigin.Begin);
            Write(Count);
            Seek(0, SeekOrigin.End);

            byte[] snapshotData = GetSnapshotContent(snapshot);

            Write(snapshotData, 0, snapshotData.Length);

            snapshot.Dispose();

            return this;
        }

        private static byte[] GetSnapshotContent(IFFSnapshot snapshot)
        {
            byte[] snapshotBuffer = snapshot.Buffer;

            return snapshotBuffer.GetRange(SnapshotContentOffset, snapshotBuffer.Length - SnapshotContentOffset).ToArray();
        }
    }
}
