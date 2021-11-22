using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World.Party
{
    public class PartyChangeNamePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the player id.
        /// </summary>
        public uint PlayerId { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            PlayerId = packet.Read<uint>();
            Name = packet.Read<string>();
        }
    }
}