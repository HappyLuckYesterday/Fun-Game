using Rhisis.Protocol.Abstractions;
using System.Collections.Generic;

namespace Rhisis.Protocol.Packets.Client.World
{
    public class DoUseSkillPointsPacket : IPacketDeserializer
    {
        private readonly Dictionary<int, int> _skills;

        /// <summary>
        /// Creates a new <see cref="DoUseSkillPointsPacket"/> instance.
        /// </summary>
        public DoUseSkillPointsPacket()
        {
            _skills = new Dictionary<int, int>();
        }

        /// <summary>
        /// Gets the skills to be updated.
        /// </summary>
        public IReadOnlyDictionary<int, int> Skills => _skills;

        /// <inheritdoc />
        public void Deserialize(IFFPacket packet)
        {
            while (!packet.IsEndOfStream)
            {
                int skillId = packet.Read<int>();
                int skillLevel = packet.Read<int>();

                if (skillId != -1)
                {
                    _skills.TryAdd(skillId, skillLevel);
                }
            }
        }
    }
}