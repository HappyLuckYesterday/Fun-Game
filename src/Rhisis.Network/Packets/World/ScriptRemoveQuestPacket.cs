using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class ScriptRemoveQuestPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the gold.
        /// </summary>
        public int QuestId { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            QuestId = packet.Read<int>();
        }
    }
}