using LiteNetwork.Protocol;
using LiteNetwork.Protocol.Abstractions;
using System;

namespace Rhisis.Protocol
{
    public class FlyffPacketProcessor : LitePacketProcessor
    {
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

        public override ILitePacketStream CreatePacket(byte[] buffer) => new FFPacket(buffer);
    }
}
