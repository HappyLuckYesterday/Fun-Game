using Rhisis.Abstractions.Protocol;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class ModifyStatusPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the strength.
        /// </summary>
        public ushort Strength { get; private set; }

        /// <summary>
        /// Gets the stamina.
        /// </summary>
        public ushort Stamina { get; private set; }

        /// <summary>
        /// Gets the dexterity.
        /// </summary>
        public ushort Dexterity { get; private set; }

        /// <summary>
        /// Gets the intelligence.
        /// </summary>
        public ushort Intelligence { get; private set; }

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            Strength = (ushort)packet.ReadInt32();
            Stamina = (ushort)packet.ReadInt32();
            Dexterity = (ushort)packet.ReadInt32();
            Intelligence = (ushort)packet.ReadInt32();
        }
    }
}
