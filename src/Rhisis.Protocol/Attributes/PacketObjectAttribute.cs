using System;

namespace Rhisis.Protocol.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class PacketObjectAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class PacketFieldAttribute : Attribute
{
    public int Order { get; set; }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class PacketFieldSize : Attribute
{
    public int Size { get; set; }
}
