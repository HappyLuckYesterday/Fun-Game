namespace Rhisis.Network
{
    /// <summary>
    /// Represents a FlyFF snapshot packet.
    /// </summary>
    public class FFSnapshot : FFPacket
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
        /// Creates a new <see cref="FFSnapshot"/> instance.
        /// </summary>
        /// <param name="snapshot">Snapshot type.</param>
        /// <param name="objectId">Target object id.</param>
        public FFSnapshot(SnapshotType snapshot, int objectId)
            : base(PacketType.SNAPSHOT)
        {
            Write(0); // Not used.
            Write(1); // Snapshot amount.
            Write(objectId);
            Write((short)snapshot);
        }
    }
}
