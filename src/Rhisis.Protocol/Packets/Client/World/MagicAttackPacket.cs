using Rhisis.Game.Common;
using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class MagicAttackPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the attack message.
        /// </summary>
        public ObjectMessageType AttackMessage { get; private set; }

        /// <summary>
        /// Gets the target object id.
        /// </summary>
        public uint TargetObjectId { get; private set; }

        /// <summary>
        /// Gets the second parameter.
        /// </summary>
        public int Parameter2 { get; private set; }

        /// <summary>
        /// Gets the third parameter.
        /// </summary>
        public int Parameter3 { get; private set; }

        /// <summary>
        /// Gets the magic power.
        /// </summary>
        public int MagicPower { get; private set; }

        /// <summary>
        /// Gets the SFX projectile id.
        /// </summary>
        public int ProjectileId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            AttackMessage = (ObjectMessageType)packet.Read<uint>();
            TargetObjectId = packet.Read<uint>();
            Parameter2 = packet.Read<int>();
            Parameter3 = packet.Read<int>();
            MagicPower = packet.Read<int>();
            ProjectileId = packet.Read<int>();
        }
    }
}
