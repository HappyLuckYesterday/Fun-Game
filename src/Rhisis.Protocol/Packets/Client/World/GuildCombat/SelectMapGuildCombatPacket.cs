using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World.GuildCombat
{
    public class SelectMapGuildCombatPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the map id.
        /// </summary>
        public int Map { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            Map = packet.Read<int>();
        }
    }
}