namespace Rhisis.Protocol.Generators.Constants;

internal static class PacketDispatcherConstants
{
    public static readonly string PacketDispatcherClassName = "PacketDispatcher";
    public static readonly string SnapshotDispatcherClassName = "SnapshotDispatcher";
    public static readonly string ExecuteMethodName = "Execute";
    public static readonly string OnBeforeExecuteMethodName = "OnBeforeExecute";
    public static readonly string OnAfterExecuteMethodName = "OnAfterExecute";
    public static readonly string OnHandlerNotImplemented = "OnHandlerNotImplemented";

    public static readonly string FFUserConnectionTypeName = "Rhisis.Protocol.FFUserConnection";
    public static readonly string FFPacketTypeName = "Rhisis.Protocol.FFPacket";
    public static readonly string IPacketHandlerTypeName = "Rhisis.Protocol.Handlers.IPacketHandler";
    public static readonly string PacketTypeName = "Rhisis.Protocol.PacketType";
    public static readonly string SnapshotTypeName = "Rhisis.Protocol.SnapshotType";
    public static readonly string ProtocolNamspace = "Rhisis.Protocol";

    public static readonly string PacketHandlerAttributeName = "PacketHandler";
    public static readonly string SnapshotHandlerAttributeName = "SnapshotHandler";
    public static readonly string CoreHandlerAttributeName = "CoreHandler";

    public static readonly string ServiceProviderTypeName = "System.IServiceProvider";
    public static readonly string ActivatorUtilitiesClassName = "Microsoft.Extensions.DependencyInjection.ActivatorUtilities";
    public static readonly string CreateInstanceMethodName = "CreateInstance";
}
