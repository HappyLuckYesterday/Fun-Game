using System;
using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class SetDestPositionPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the X coordinate.
    /// </summary>
    public float X { get; private set; }

    /// <summary>
    /// Gets the Y coordinate.
    /// </summary>
    public float Y { get; private set; }

    /// <summary>
    /// Gets the Z coordinate.
    /// </summary>
    public float Z { get; private set; }

    /// <summary>
    /// Gets the forward state.
    /// </summary>
    public bool Forward { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        X = packet.ReadSingle();
        Y = packet.ReadSingle();
        Z = packet.ReadSingle();
        Forward = Convert.ToBoolean(packet.ReadByte());
    }
}
