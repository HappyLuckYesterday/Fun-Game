using Rhisis.Game.Abstractions.Protocol;
using Sylver.Network.Data;

namespace Rhisis.Network.Packets.World
{
    public class DoUseItemPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets or sets the item index in the inventory.
        /// </summary>
        public int ItemIndex { get; private set; }

        /// <summary>
        /// Gets or sets the object id.
        /// </summary>
        public int ObjectId { get; private set; }

        /// <summary>
        /// Gets or sets the body part.
        /// </summary>
        public int Part { get; private set; }

        /// <summary>
        /// Gets or sets the item flight speed.
        /// </summary>
        public float FlightSpeed { get; private set; }

        /// <inheritdoc />
        public void Deserialize(INetPacketStream packet)
        {
            ItemIndex = (packet.Read<int>() >> 16) & 0xFFFF;
            ObjectId = packet.Read<int>();
            Part = packet.Read<int>();

            if (!packet.IsEndOfStream)
            {
                FlightSpeed = packet.Read<float>();
            }
        }
    }
}