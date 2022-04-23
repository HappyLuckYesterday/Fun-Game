namespace Rhisis.Protocol.Core
{
    /// <summary>
    /// Defines the core server/client packet types.
    /// </summary>
    public enum CorePacketType : int
    {
        Welcome,
        Authenticate,
        AuthenticationResult,
        UpdateClusterWorldsList,
        UpdateCluster,
        DisconnectUserFromCluster,
        BroadcastMessage,
        PlayerConnectedToChannel,
        PlayerDisconnectedFromChannel
    }
}
