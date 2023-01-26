using Rhisis.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhisis.Protocol.Handlers;

public static class PacketHandlerCache
{
    private static Dictionary<PacketType, Type> _packetTypes;
    private static Dictionary<SnapshotType, Type> _snapshotTypes;

    public static void Load(Type messageHandlerBaseType)
    {
        _packetTypes = Assembly.GetEntryAssembly()
            .GetTypes()
            .Where(x => x.IsClass && x.ImplementsInterface(messageHandlerBaseType) && x.GetCustomAttribute<PacketHandlerAttribute>() != null)
            .ToDictionary(
                x => x.GetCustomAttribute<PacketHandlerAttribute>().PacketType,
                x => x.GetInterfaces().Single(i => i.GetGenericTypeDefinition() == messageHandlerBaseType).GetGenericArguments()[0]);

        _snapshotTypes = Assembly.GetEntryAssembly()
            .GetTypes()
            .Where(x => x.IsClass && x.ImplementsInterface(messageHandlerBaseType) && x.GetCustomAttribute<SnapshotHandlerAttribute>() != null)
            .ToDictionary(
                x => x.GetCustomAttribute<SnapshotHandlerAttribute>().SnapshotType,
                x => x.GetInterfaces().Single(i => i.GetGenericTypeDefinition() == messageHandlerBaseType).GetGenericArguments()[0]);
    }

    public static Type GetPacketType(PacketType packetTypeHeader)
        => _packetTypes.TryGetValue(packetTypeHeader, out Type packetType) ? packetType : null;

    public static Type GetSnapshotType(SnapshotType snapshotTypeHeader)
        => _snapshotTypes.TryGetValue(snapshotTypeHeader, out Type snapshotType) ? snapshotType : null;
}