using Sylver.Network.Data;
using System.IO;
using System.Text;

namespace Rhisis.Network
{
    /// <summary>
    /// Represents a FlyFF packet.
    /// </summary>
    public class FFPacket : NetPacketStream
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

        /// <summary>
        /// Write packet header.
        /// </summary>
        /// <param name="packetHeader">FFPacket header</param>
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

        /// <summary>
        /// Start a new merged packet.
        /// </summary>
        /// <param name="moverId"></param>
        /// <param name="command"></param>
        /// <param name="mainCommand"></param>
        public void StartNewMergedPacket(uint moverId, object command, uint mainCommand)
        {
            var packet = (uint)command;

            if (_mergedPacketCount == 0)
            {
                Write((int)mainCommand);
                Write(0);
                Write(++_mergedPacketCount);
            }
            else
            {
                Seek(13, SeekOrigin.Begin);
                Write(++_mergedPacketCount);
                Seek(0, SeekOrigin.End);
            }

            Write(moverId);
            Write((short)packet);
        }

        /// <summary>
        /// Start a new merged packet.
        /// </summary>
        /// <param name="moverId"></param>
        /// <param name="command"></param>
        public void StartNewMergedPacket(uint moverId, object command) => StartNewMergedPacket(moverId, command, 0xFFFFFF00);
    }
}
