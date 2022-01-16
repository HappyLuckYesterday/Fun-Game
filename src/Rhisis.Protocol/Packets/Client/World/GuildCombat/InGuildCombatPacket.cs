using Rhisis.Abstractions.Protocol;
using Rhisis.Game.Common;

namespace Rhisis.Protocol.Packets.Client.World.GuildCombat
{
    public class InGuildCombatPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the guild combat type.
        /// </summary>
        public GuildCombatType GuildCombatType { get; private set; }

        /// <summary>
        /// Gets the guild combat gold amount.
        /// </summary>
        public uint? Penya { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            GuildCombatType = (GuildCombatType)packet.ReadInt32();
            Penya = GuildCombatType == GuildCombatType.GC_IN_APP ? (uint?)packet.ReadUInt32() : null;
        }
    }
}