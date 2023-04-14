using Rhisis.Core.IO;
using System;
using System.Text;

namespace Rhisis.Protocol;

/// <summary>
/// Represents a FlyFF packet.
/// </summary>
public class FFPacket : BinaryStream
{
    private const int CodePage = 1252;
    private static readonly Encoding StringEncoding = Encoding.GetEncoding(CodePage);

    /// <summary>
    /// Gets the packet header.
    /// </summary>
    public PacketType Header { get; }

    /// <summary>
    /// Creates a new empty <see cref="FFPacket"/>.
    /// </summary>
    public FFPacket()
    {
        WriteByte(FlyffPacketProcessor.HeaderNumber);
        WriteInt32(0);
    }

    /// <summary>
    /// Creates a new <see cref="FFPacket"/> with a header.
    /// </summary>
    /// <param name="packetHeader"></param>
    public FFPacket(PacketType packetHeader)
        : this()
    {
        Header = packetHeader;
        WriteUInt32((uint)packetHeader);
    }

    /// <summary>
    /// Creates a new <see cref="FFPacket"/> with an existing content.
    /// </summary>
    /// <param name="packetBuffer"></param>
    /// <param name="ignorePacketHeader">Boolean value that indicates if the packet header should be ignored during packet initialization.</param>
    public FFPacket(byte[] packetBuffer, bool ignorePacketHeader = false)
        : base(packetBuffer) 
    {
        if (!ignorePacketHeader)
        {
            Header = (PacketType)ReadUInt32();
        }
    }

    /// <summary>
    /// Reads a string from the packet.
    /// </summary>
    /// <returns>String value.</returns>
    public override string ReadString()
    {
        int stringLength = ReadInt32();
        byte[] stringBytes = ReadBytes(stringLength);

        return StringEncoding.GetString(stringBytes);
    }

    /// <summary>
    /// Writes a string value at the end of the packet.
    /// </summary>
    /// <param name="value">String value to write.</param>
    public override void WriteString(string value)
    {
        string stringValue = value?.ToString() ?? string.Empty;

        WriteInt32(stringValue.Length);

        if (stringValue.Length > 0)
        {
            WriteBytes(StringEncoding.GetBytes(stringValue));
        }
    }
}
