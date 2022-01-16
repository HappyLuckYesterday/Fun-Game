using Rhisis.Abstractions.Protocol;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class RangeAttackPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the attack message.
        /// </summary>
        public ObjectMessageType AttackMessage { get; private set; }

        /// <summary>
        /// Gets the object id.
        /// </summary>
        public uint ObjectId { get; private set; }

        /// <summary>
        /// Gets the range attack power.
        /// </summary>
        public int Power { get; private set; }

        /// <summary>
        /// Gets the projectile id.
        /// </summary>
        public int ProjectileId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            AttackMessage = (ObjectMessageType)packet.ReadUInt32();
            ObjectId = packet.ReadUInt32();
            Power = packet.ReadInt32();
            ProjectileId = packet.ReadInt32();
        }
    }
}
