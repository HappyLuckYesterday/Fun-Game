using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Friends;

/// <summary>
/// Provides a data structure representing a friend state interception request packet.
/// </summary>
public class FriendInterceptStatePacket
{
    /// <summary>
    /// Gets the current player id that is intercepting the request.
    /// </summary>
    public uint CurrentPlayerId { get; private set; }

    /// <summary>
    /// Gets the target friend player id.
    /// </summary>
    public uint FriendPlayerId { get; private set; }

    public FriendInterceptStatePacket(FFPacket packet)
    {
        CurrentPlayerId = packet.ReadUInt32();
        FriendPlayerId = packet.ReadUInt32();
    }
}
