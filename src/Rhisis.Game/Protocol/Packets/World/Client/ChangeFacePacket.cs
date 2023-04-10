using Rhisis.Protocol;
using System;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class ChangeFacePacket
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

    public ChangeFacePacket(FFPacket packet)
    {
        ObjectId = packet.ReadUInt32();
        FaceNumber = packet.ReadUInt32();
        Cost = packet.ReadInt32();
        UseCoupon = Convert.ToBoolean(packet.ReadInt32());
    }
}