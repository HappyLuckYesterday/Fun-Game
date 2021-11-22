using Rhisis.Game.Abstractions.Protocol;
using LiteNetwork.Protocol.Abstractions;

namespace Rhisis.Network.Packets.World
{
    public class SetQuestPacket : IPacketDeserializer
    {
        /// <summary>
        /// Gets the quest id.
        /// </summary>
        public int QuestId { get; private set; }

        /// <summary>
        /// Gets the state.
        /// </summary>
        public int State { get; private set; }

        /// <inheritdoc />
        public void Deserialize(ILitePacketStream packet)
        {
            QuestId = packet.Read<int>();
            State = packet.Read<int>();
        }
    }
}