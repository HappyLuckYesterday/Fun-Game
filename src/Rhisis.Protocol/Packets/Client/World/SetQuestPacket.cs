using Rhisis.Protocol.Abstractions;

namespace Rhisis.Protocol.Packets.Client.World
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
        public void Deserialize(IFFPacket packet)
        {
            QuestId = packet.Read<int>();
            State = packet.Read<int>();
        }
    }
}