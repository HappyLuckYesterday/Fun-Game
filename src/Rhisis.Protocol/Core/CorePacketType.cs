namespace Rhisis.Protocol.Core
{
    /// <summary>
    /// Defines the core server/client packet types.
    /// </summary>
    public enum CorePacketType : int
    {
        Welcome,
        AuthenticationRequest,
        AuthenticationResult,
        UpdateClusterWorldsList,
        UpdateCluster,
        UpdateClusterWorldChannel,
        RemoveClusterWorldChannel,
        DisconnectUserFromCluster,
        BroadcastMessage,
        PlayerConnectedToChannel,
        PlayerDisconnectedFromChannel
    }
}
