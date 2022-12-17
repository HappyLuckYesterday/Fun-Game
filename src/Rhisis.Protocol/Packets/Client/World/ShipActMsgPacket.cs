using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class ShipActMsgPacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the message.
    /// </summary>
    public uint Message { get; private set; }

    /// <summary>
    /// Gets the first parameter.
    /// </summary>
    public int Parameter1 { get; private set; }

    /// <summary>
    /// Gets the second parameter.
    /// </summary>
    public int Parameter2 { get; private set; }

    /// <summary>
    /// Gets the ship.
    /// </summary>
    public uint Ship { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        Message = packet.ReadUInt32();
        Parameter1 = packet.ReadInt32();
        Parameter2 = packet.ReadInt32();
        Ship = packet.ReadUInt32();
    }
}