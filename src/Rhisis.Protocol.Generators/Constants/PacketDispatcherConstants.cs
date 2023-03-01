using Microsoft.CodeAnalysis;

namespace Rhisis.Protocol.Generators.Constants;

internal static class PacketDispatcherConstants
{
    public const string PacketDispatcherClassName = "PacketDispatcher";
    public const string SnapshotDispatcherClassName = "SnapshotDispatcher";
    public const string ExecuteMethodName = "Execute";
    public const string OnBeforeExecuteMethodName = "OnBeforeExecute";
    public const string OnAfterExecuteMethodName = "OnAfterExecute";
    public const string OnHandlerNotImplemented = "OnHandlerNotImplemented";

    public const string FFUserConnectionTypeName = "Rhisis.Protocol.FFUserConnection";
    public const string FFPacketTypeName = "Rhisis.Protocol.FFPacket";
    public const string IPacketHandlerTypeName = "Rhisis.Protocol.Handlers.IPacketHandler";
    public const string PacketTypeName = "Rhisis.Protocol.PacketType";
    public const string SnapshotTypeName = "Rhisis.Protocol.SnapshotType";
    public const string ProtocolNamspace = "Rhisis.Protocol";

    public const string PacketHandlerAttributeName = "PacketHandler";
    public const string SnapshotHandlerAttributeName = "SnapshotHandler";

    public const string ServiceProviderTypeName = "System.IServiceProvider";
    public const string ActivatorUtilitiesClassName = "Microsoft.Extensions.DependencyInjection.ActivatorUtilities";
    public const string CreateInstanceMethodName = "CreateInstance";
}

internal static class Diagnostics
{
    public static DiagnosticDescriptor MissingExecuteMethod(string packetType)
    {
        return new("RHIGEN001",
            title: "Missing Execute method",
            messageFormat: $"No execute method found for packet handler '{packetType}'.",
            category: "Packet Dispatcher Generator",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);
    }
}