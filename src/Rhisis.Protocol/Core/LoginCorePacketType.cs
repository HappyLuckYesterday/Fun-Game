namespace Rhisis.Protocol.Core
{
    /// <summary>
    /// Defines the login core server/client packet types.
    /// </summary>
    public enum LoginCorePacketType : int
    {
        Welcome,
        Authenticate,
        AuthenticationResult,
        UpdateClusterWorldsList,
        UpdateCluster,
        DisconnectUserFromCluster
    }
}
