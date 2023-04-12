using Rhisis.Game.Common;
using Rhisis.Protocol;

namespace Rhisis.Game.Protocol.Packets.World.Client.Friends;

/// <summary>
/// Provides a data structure representing a set messenger status request and response.
/// </summary>
public class SetFriendStatePacket
{
    /// <summary>
    /// Gets the player id.
    /// </summary>
    public int PlayerId { get; private set; }

    /// <summary>
    /// Gets the player status.
    /// </summary>
    public MessengerStatusType Status { get; private set; }

    public SetFriendStatePacket(FFPacket packet)
    {
        PlayerId = packet.ReadInt32();
        Status = (MessengerStatusType)packet.ReadInt32();
    }
}
