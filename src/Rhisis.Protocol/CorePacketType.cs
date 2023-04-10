namespace Rhisis.Protocol;

public enum CorePacketType : uint
{
    Handshake = 0,
    Authenticate,
    AuthenticationResult,
    UpdateClusterInfo,
    AddChannel,
    UpdateChannel,
    RemoveChannel,
    ChannelConfiguration
}
