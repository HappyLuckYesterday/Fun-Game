using System;

namespace Rhisis.Protocol.Handlers;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class CoreHandlerAttribute : Attribute
{
    public CorePacketType CorePacketType { get; }

    public CoreHandlerAttribute(CorePacketType corePacketType)
    {
        CorePacketType = corePacketType;
    }
}
