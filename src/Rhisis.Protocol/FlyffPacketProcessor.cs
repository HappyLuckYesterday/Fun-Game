using LiteNetwork.Protocol;
using System;

namespace Rhisis.Protocol
{
    public class FlyffPacketProcessor : LitePacketProcessor
    {
        private const byte HeaderNumber = 0x5E;
        private const byte PacketHeaderSize = sizeof(byte) + sizeof(int);

        /// <summary>
        /// Gets the FlyFF packet header size.
        /// </summary>
        public override int HeaderSize => 13;

        public override bool IncludeHeader => false;

        public override int GetMessageLength(byte[] buffer)
        {
            if (buffer[0] == FFPacket.Header)
            {
                var packetDataLength = new byte[4];

                Buffer.BlockCopy(buffer, 5, packetDataLength, 0, packetDataLength.Length);

                return BitConverter.ToInt32(packetDataLength, 0);
            }

            return 0;
        }

        public override byte[] AppendHeader(byte[] buffer)
        {
            int contentLength = buffer.Length;
            byte[] contentLengthBuffer = BitConverter.GetBytes(contentLength);
            byte[] packetBuffer = new byte[PacketHeaderSize + buffer.Length];

            packetBuffer[0] = HeaderNumber;
            Array.Copy(contentLengthBuffer, 0, packetBuffer, 1, sizeof(int));
            Array.Copy(buffer, 0, packetBuffer, PacketHeaderSize, contentLength);

            return packetBuffer;
        }
    }
}
