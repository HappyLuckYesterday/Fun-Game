using System;

namespace Rhisis.Protocol.Handlers;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class SnapshotHandlerAttribute : Attribute
{
    public SnapshotType SnapshotType { get; }

    public SnapshotHandlerAttribute(SnapshotType snapshotType)
    {
        SnapshotType = snapshotType;
    }
}
