using System;
using System.IO;

namespace Rhisis.Abstractions.Protocol
{
    /// <summary>
    /// Provides an interface to explorer flyff packets.
    /// </summary>
    public interface IFFPacket : IDisposable
    {
        /// <summary>
        /// Gets the packet buffer.
        /// </summary>
        byte[] Buffer { get; }

        /// <summary>
        /// Gets a boolean value that indicates if the current packet stream has reached end of stream.
        /// </summary>
        bool IsEndOfStream { get; }

        /// <summary>
        /// Write packet header.
        /// </summary>
        /// <param name="packetHeader">FFPacket header</param>
        void WriteHeader(object packetHeader);

        byte ReadByte();

        sbyte ReadSByte();

        char ReadChar();

        bool ReadBoolean();

        short ReadInt16();

        ushort ReadUInt16();

        int ReadInt32();

        uint ReadUInt32();

        long ReadInt64();

        ulong ReadUInt64();

        float ReadSingle();

        double ReadDouble();

        string ReadString();

        byte[] ReadBytes(int count);

        void WriteByte(byte value);

        void WriteSByte(sbyte value);

        void WriteChar(char value);

        void WriteBoolean(bool value);

        void WriteInt16(short value);

        void WriteUInt16(ushort value);

        void WriteInt32(int value);

        void WriteUInt32(uint value);

        void WriteSingle(float value);

        void WriteDouble(double value);

        void WriteInt64(long value);

        void WriteUInt64(ulong value);

        void WriteString(string value);

        void WriteBytes(byte[] values);

        long Seek(long offset, SeekOrigin loc);
    }
}
