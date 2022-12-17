using System;
using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World;

public class ChangeFacePacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the object id.
    /// </summary>
    public uint ObjectId { get; private set; }

    /// <summary>
    /// Gets the face number.
    /// </summary>
    public uint FaceNumber { get; private set; }

    /// <summary>
    /// Gets the cost.
    /// </summary>
    public int Cost { get; private set; }

    /// <summary>
    /// Gets if a coupon will be used.
    /// </summary>
    public bool UseCoupon { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        ObjectId = packet.ReadUInt32();
        FaceNumber = packet.ReadUInt32();
        Cost = packet.ReadInt32();
        UseCoupon = Convert.ToBoolean(packet.ReadInt32());
    }
}