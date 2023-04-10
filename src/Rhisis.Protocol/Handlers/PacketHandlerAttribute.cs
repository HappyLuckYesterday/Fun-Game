using System;

namespace Rhisis.Protocol.Handlers;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class PacketHandlerAttribute : Attribute
{
    public PacketType PacketType { get; }

    public PacketHandlerAttribute(PacketType packetType)
    {
        PacketType = packetType;
    }
}
