using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Friends;

/// <summary>
/// Provides a data structure representing the add friend request packet.
/// </summary>
public class AddFriendRequestPacket
{
    /// <summary>
    /// Gets the current player id that is initiating the friend request.
    /// </summary>
    public uint CurrentPlayerId { get; private set; }

    /// <summary>
    /// Gets the target player id that will receive the friend request.
    /// </summary>
    public uint TargetPlayerId { get; private set; }

    public AddFriendRequestPacket(FFPacket packet)
    {
        CurrentPlayerId = packet.ReadUInt32();
        TargetPlayerId = packet.ReadUInt32();
    }
}
