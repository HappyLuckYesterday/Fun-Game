using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World.Friends
{
    /// <summary>
    /// Provides a data structure when the player wants to remove a friend from its contact list.
    /// </summary>
    public class RemoveFriendPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the current player id.
        /// </summary>
        public int CurrentPlayerId { get; private set; }

        /// <summary>
        /// Gets the player's friend id to remove from its messenger list.
        /// </summary>
        public int FriendId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            CurrentPlayerId = packet.ReadInt32();
            FriendId = packet.ReadInt32();
        }
    }
}
