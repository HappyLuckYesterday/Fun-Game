using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Common;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World.Friends
{
    /// <summary>
    /// Provides a data structure representing a set messenger status request and response.
    /// </summary>
    public class SetFriendStatePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public int PlayerId { get; private set; }

        /// <summary>
        /// Gets the player status.
        /// </summary>
        public MessengerStatusType Status { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            PlayerId = packet.ReadInt32();
            Status = (MessengerStatusType)packet.ReadInt32();
        }
    }
}
