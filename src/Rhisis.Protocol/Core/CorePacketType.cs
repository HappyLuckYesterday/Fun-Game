namespace Rhisis.Protocol.Core;

/// <summary>
/// Defines the core server/client packet types.
/// </summary>
public enum CorePacketType : byte
{
    Welcome,

    // Authentication
    AuthenticationRequest,
    AuthenticationResult,

    // Cluster
    UpdateClusterWorldsList,
    UpdateCluster,
    UpdateClusterWorldChannel,
    RemoveClusterWorldChannel,
    DisconnectUserFromCluster,
    PlayerConnectedToChannel,
    PlayerDisconnectedFromChannel,
    BroadcastMessage,
}
