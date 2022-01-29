using LiteNetwork.Protocol;
using Rhisis.Abstractions.Protocol;
using System.IO;
using System.Text;

namespace Rhisis.Protocol
{
    /// <summary>
    /// Represents a FlyFF packet.
    /// </summary>
    public class FFPacket : LitePacketStream, IFFPacket
    {
        /// <summary>
        /// Gets the FlyFF packet header constant value.
        /// </summary>
        public const byte Header = 0x5E;

        /// <summary>
        /// Gets the FlyFF packet size offset in packet stream.
        /// </summary>
        public static readonly int PacketSizeOffset = sizeof(byte);

        /// <summary>
        /// Gets the FlyFF packet offset where the data starts.
        /// </summary>
        public static readonly int PacketDataStartOffset = sizeof(byte) + sizeof(int);

        private short _mergedPacketCount;

        /// <inheritdoc />
        protected override Encoding ReadEncoding => Encoding.GetEncoding(1252);

        /// <inheritdoc />
        protected override Encoding WriteEncoding => Encoding.GetEncoding(1252);

        /// <summary>
        /// Gets the FFPacket buffer.
        /// </summary>
        public override byte[] Buffer => BuildPacketBuffer();

        /// <summary>
        /// Creates a new FFPacket in write-only mode.
        /// </summary>
        public FFPacket()
        {
            base.Write(Header);
            base.Write(0);
        }

        /// <summary>
        /// Creates a new FFPacket with a header.
        /// </summary>
        /// <param name="packetHeader"></param>
        public FFPacket(object packetHeader)
            : this()
        {
            WriteHeader(packetHeader);
        }

        /// <summary>
        /// Creates a new FFPacket in read-only mode.
        /// </summary>
        /// <param name="buffer"></param>
        public FFPacket(byte[] buffer)
            : base(buffer)
        {
        }

        public void WriteHeader(object packetHeader) => Write((uint)packetHeader);

        /// <summary>
        /// Builds the packet buffer.
        /// </summary>
        /// <returns></returns>
        private byte[] BuildPacketBuffer()
        {
            long oldPointer = Position;

            Seek(PacketSizeOffset, SeekOrigin.Begin);
            base.Write((int)Length - PacketDataStartOffset);
            Seek(oldPointer, SeekOrigin.Begin);

            return base.Buffer;
        }
    }
}
