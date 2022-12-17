using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World.Friends;

/// <summary>
/// Provides a data structure representing a query to get the friends state.
/// </summary>
public class GetFriendStatePacket : IPacketDeserializer
{
    /// <summary>
    /// Gets the current player id that is requesting the friends states.
    /// </summary>
    public uint CurrentPlayerId { get; private set; }

    /// <inheritdoc />
    public void Deserialize(IFFPacket packet)
    {
        CurrentPlayerId = packet.ReadUInt32();
    }
}
