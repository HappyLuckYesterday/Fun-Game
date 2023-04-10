using Rhisis.Protocol;
using System;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class SetHairPacket
{
    /// <summary>
    /// Gets the hair id.
    /// </summary>
    public byte HairId { get; private set; }

    /// <summary>
    /// Gets the red color.
    /// </summary>
    public byte R { get; private set; }

    /// <summary>
    /// Gets the green color.
    /// </summary>
    public byte G { get; private set; }

    /// <summary>
    /// Gets the blue color.
    /// </summary>
    public byte B { get; private set; }

    /// <summary>
    /// Gets if a coupon will be used.
    /// </summary>
    public bool UseCoupon { get; private set; }

    public SetHairPacket(FFPacket packet)
    {
        HairId = packet.ReadByte();
        R = packet.ReadByte();
        G = packet.ReadByte();
        B = packet.ReadByte();
        UseCoupon = Convert.ToBoolean(packet.ReadInt32());
    }
}
