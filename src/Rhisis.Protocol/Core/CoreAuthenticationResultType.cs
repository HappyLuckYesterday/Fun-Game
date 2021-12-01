namespace Rhisis.Protocol.Core
{
    /// <summary>
    /// Defines core server/client authentication result types.
    /// </summary>
    public enum CoreAuthenticationResultType
    {
        Success,
        FailedUnknownServer,
        FailedClusterExists,
        FailedWorldExists,
        FailedWrongPassword
    }
}
