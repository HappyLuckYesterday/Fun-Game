namespace Rhisis.Protocol;

public enum CorePacketType : uint
{
    Handshake = 0,
    Authenticate,
    AuthenticationResult,
    AddChannel,
    UpdateChannel,
    RemoveChannel,
    UpdateClusterInfo
}
