using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Friends;

/// <summary>
/// Provides a data structure representing a query to get the friends state.
/// </summary>
public class GetFriendStatePacket
{
    /// <summary>
    /// Gets the current player id that is requesting the friends states.
    /// </summary>
    public uint CurrentPlayerId { get; private set; }

    public GetFriendStatePacket(FFPacket packet)
    {
        CurrentPlayerId = packet.ReadUInt32();
    }
}
