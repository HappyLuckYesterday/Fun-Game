using Rhisis.Core.Extensions;
using Rhisis.Protocol;
using System.Collections.Generic;

namespace Rhisis.Game.Protocol.Packets.World.Client;

public class DoUseSkillPointsPacket
{
    private readonly Dictionary<int, int> _skills = new();

    /// <summary>
    /// Gets the skills to be updated.
    /// </summary>
    public IReadOnlyDictionary<int, int> Skills => _skills;

    public DoUseSkillPointsPacket(FFPacket packet)
    {
        while (!packet.IsEndOfStream())
        {
            int skillId = packet.ReadInt32();
            int skillLevel = packet.ReadInt32();

            if (skillId != -1)
            {
                _skills.TryAdd(skillId, skillLevel);
            }
        }
    }
}