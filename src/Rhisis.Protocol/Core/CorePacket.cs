using System;
using System.IO;
using System.Text;

namespace Rhisis.Protocol.Core
{
    public class CorePacket : MemoryStream
    {
        private const int CodePage = 1252;
        private static readonly Encoding StringEncoding = Encoding.GetEncoding(CodePage);

        private readonly BinaryReader _reader;
        private readonly BinaryWriter _writer;

        /// <summary>
        /// Creates a new <see cref="CorePacket"/> in write-only mode.
        /// </summary>
        public CorePacket()
        {
            _writer = new BinaryWriter(this);
        }

        /// <summary>
        /// Creates a new <see cref="CorePacket"/> in read-only mode.
        /// </summary>
        /// <param name="buffer"></param>
        public CorePacket(byte[] buffer)
            : base(buffer)
        {
            _reader = new BinaryReader(this);
        }

        public new byte ReadByte()
        {
            CheckIfReadMode();
            return _reader.ReadByte();
        }

        public sbyte ReadSByte()
        {
            CheckIfReadMode();
            return _reader.ReadSByte();
        }

        public char ReadChar()
        {
            CheckIfReadMode();
            return _reader.ReadChar();
        }

        public bool ReadBoolean()
        {
            CheckIfReadMode();
            return _reader.ReadBoolean();
        }

        public short ReadInt16()
        {
            CheckIfReadMode();
            return _reader.ReadInt16();
        }

        public ushort ReadUInt16()
        {
            CheckIfReadMode();
            return _reader.ReadUInt16();
        }

        public int ReadInt32()
        {
            CheckIfReadMode();
            return _reader.ReadInt32();
        }

        public uint ReadUInt32()
        {
            CheckIfReadMode();
            return _reader.ReadUInt32();
        }

        public long ReadInt64()
        {
            CheckIfReadMode();
            return _reader.ReadInt64();
        }

        public ulong ReadUInt64()
        {
            CheckIfReadMode();
            return _reader.ReadUInt64();
        }

        public float ReadSingle()
        {
            CheckIfReadMode();
            return _reader.ReadSingle();
        }

        public double ReadDouble()
        {
            CheckIfReadMode();
            return _reader.ReadDouble();
        }

        public string ReadString()
        {
            CheckIfReadMode();
            int stringLength = ReadInt32();
            byte[] stringBytes = ReadBytes(stringLength);

            return StringEncoding.GetString(stringBytes);
        }

        public byte[] ReadBytes(int count)
        {
            CheckIfReadMode();
            return _reader.ReadBytes(count);
        }

        public new void WriteByte(byte value)
        {
            CheckIfWriteMode();
            _writer.Write(value);
        }

        public void WriteSByte(sbyte value)
        {
            CheckIfWriteMode();
            _writer.Write(value);
        }

        public void WriteChar(char value)
        {
            CheckIfWriteMode();
            _writer.Write(value);
        }

        public void WriteBoolean(bool value)
        {
            CheckIfWriteMode();
            _writer.Write(value);
        }

        public void WriteInt16(short value)
        {
            CheckIfWriteMode();
            _writer.Write(value);
        }

        public void WriteUInt16(ushort value)
        {
            CheckIfWriteMode();
            _writer.Write(value);
        }

        public void WriteInt32(int value)
        {
            CheckIfWriteMode();
            _writer.Write(value);
        }

        public void WriteUInt32(uint value)
        {
            CheckIfWriteMode();
            _writer.Write(value);
        }

        public void WriteSingle(float value)
        {
            CheckIfWriteMode();
            _writer.Write(value);
        }

        public void WriteDouble(double value)
        {
            CheckIfWriteMode();
            _writer.Write(value);
        }

        public void WriteInt64(long value)
        {
            CheckIfWriteMode();
            _writer.Write(value);
        }

        public void WriteUInt64(ulong value)
        {
            CheckIfWriteMode();
            _writer.Write(value);
        }

        public void WriteString(string value)
        {
            CheckIfWriteMode();
            string stringValue = value?.ToString() ?? string.Empty;

            _writer.Write(stringValue.Length);

            if (stringValue.Length > 0)
            {
                _writer.Write(StringEncoding.GetBytes(stringValue));
            }
        }

        public void WriteBytes(byte[] values)
        {
            CheckIfWriteMode();
            _writer.Write(values);
        }

        private void CheckIfReadMode()
        {
            if (_reader is null)
            {
                throw new InvalidOperationException("Packet is not in read mode.");
            }
        }

        private void CheckIfWriteMode()
        {
            if (_writer is null)
            {
                throw new InvalidOperationException("Packet is not in write mode.");
            }
        }
    }
}
