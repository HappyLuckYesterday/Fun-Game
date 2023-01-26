using System;
using LiteNetwork.Protocol;

namespace Rhisis.Protocol;

public sealed class FlyffPacketProcessor : LitePacketProcessor
{
    internal const byte HeaderNumber = 0x5E;
    internal const byte PacketDataStartOffset = sizeof(byte) + sizeof(int);

    /// <summary>
    /// Gets the FlyFF packet header size.
    /// </summary>
    public override int HeaderSize => 13;

    public override bool IncludeHeader => false;

    public override int GetMessageLength(byte[] buffer)
    {
        if (buffer[0] != HeaderNumber)
        {
            throw new InvalidOperationException($"Packet header '0x{buffer[0]:X2}' is invalid. (Excepted header: 0x{HeaderNumber:X2})");
        }

        var packetDataLength = new byte[4];

        Buffer.BlockCopy(buffer, 5, packetDataLength, 0, packetDataLength.Length);

        return BitConverter.ToInt32(packetDataLength, 0);
    }

    public override byte[] AppendHeader(byte[] buffer)
    {
        byte[] contentLengthBuffer = BitConverter.GetBytes(buffer.Length - PacketDataStartOffset);

        Array.Copy(contentLengthBuffer, 0, buffer, 1, sizeof(int));

        return buffer;
    }
}
