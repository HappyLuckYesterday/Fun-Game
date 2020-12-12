using Rhisis.Game.Abstractions.Protocol;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World.Friends
{
    /// <summary>
    /// Provides a data structure representing a friend state interception request packet.
    /// </summary>
    public class FriendInterceptStatePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the current player id that is intercepting the request.
        /// </summary>
        public uint CurrentPlayerId { get; private set; }

        /// <summary>
        /// Gets the target friend player id.
        /// </summary>
        public uint FriendPlayerId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            CurrentPlayerId = packet.ReadUInt32();
            FriendPlayerId = packet.ReadUInt32();
        }
    }
}
