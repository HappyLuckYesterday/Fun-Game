namespace Rhisis.Protocol.Core
{
    /// <summary>
    /// Defines the cluster core server/client packet types.
    /// </summary>
    public enum ClusterCorePacketType : int
    {
        Welcome,
        Authenticate,
        AuthenticationResult,
        UpdateWorldChannel,
    }
}
