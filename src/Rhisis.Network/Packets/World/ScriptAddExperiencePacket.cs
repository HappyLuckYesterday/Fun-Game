using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class ScriptAddExperiencePacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public long Experience { get; private set; }

        /// <inheritdoc />
        public void Deserialize (ILitePacketStream packet)
        {
            Experience = packet.Read<long>();
        }
    }
}