using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Game;
using Rhisis.Abstractions.Entities;
using Rhisis.Abstractions.Resources;
using Rhisis.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Protocol.Snapshots;
using Rhisis.Protocol.Snapshots.Skills;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Client.World;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.WorldServer.Handlers;

[Handler]
public class BuffPangHandler
{
    private readonly ILogger<BuffPangHandler> _logger;
    private readonly IOptions<WorldOptions> _worldConfiguration;
    private readonly IGameResources _gameResources;
    private readonly ISkillSystem _skillSystem;

    public BuffPangHandler(ILogger<BuffPangHandler> logger, IOptions<WorldOptions> worldConfiguration, IGameResources gameResources, ISkillSystem skillSystem)
    {
        _logger = logger;
        _worldConfiguration = worldConfiguration;
        _gameResources = gameResources;
        _skillSystem = skillSystem;
    }

    [HandlerAction(PacketType.NPC_BUFF)]
    public void OnNpcBuff(IPlayer player, NpcBuffPacket packet)
    {
        NpcBuffOptions buffConfiguration = _worldConfiguration.Value.NpcBuff.Get(packet.NpcKey);

        if (buffConfiguration is null)
        {
            _logger.LogWarning($"Failed to find npc buff configuration with key '{packet.NpcKey}' or default.");
            return;
        }

        if (player.Level < buffConfiguration.Min || player.Level > buffConfiguration.Max)
        {
            using var defineTextSnapshot = new DefinedTextSnapshot(player, DefineText.TID_GAME_NPCBUFF_LEVELLIMIT,
                $"{buffConfiguration.Min}", $"{buffConfiguration.Max}", $"\"\"");
            player.Send(defineTextSnapshot);
            return;
        }

        INpc buffer = player.VisibleObjects.OfType<INpc>().FirstOrDefault(x => x.Name == packet.NpcKey);
        
        if (buffer is null)
        {
            throw new InvalidOperationException($"Failed to find NPC buffer with name: '{packet.NpcKey}'.");
        }

        foreach (var buff in buffConfiguration.Buffs)
        {
            SkillData skillData = GetSkillData(buff.SkillId);

            if (skillData is null)
            {
                continue;
            }

            int buffLevel = Math.Clamp(buff.Level, 1, skillData.MaxLevel);
            SkillLevelData skillLevelData = skillData.SkillLevels.GetValueOrDefault(buffLevel);

            if (skillLevelData is null)
            {
                _logger.LogWarning($"Cannot find skill level data for skill: {skillData.Name} and level {buffLevel}");
                continue;
            }

            var attributes = new Dictionary<DefineAttributes, int>();

            if (skillLevelData.DestParam1 > 0)
            {
                attributes.Add(skillLevelData.DestParam1, skillLevelData.DestParam1Value);
            }
            if (skillLevelData.DestParam2 > 0)
            {
                attributes.Add(skillLevelData.DestParam2, skillLevelData.DestParam2Value);
            }

            var buffSkill = new BuffSkill(player, attributes, skillData, buffLevel)
            {
                RemainingTime = buff.Time * 1000
            };

            _skillSystem.ApplyBuff(player, buffSkill);

            using var snapshot = new DoApplyUseSkillSnapshot(player, player.Id, skillData.Id, buffLevel);
            player.Send(snapshot);
            player.SendToVisible(snapshot);
        }
    }

    private SkillData GetSkillData(string nameOrId)
    {
        if (!int.TryParse(nameOrId, out int skillId))
        {
            if (!_gameResources.Defines.TryGetValue(nameOrId, out skillId))
            {
                _logger.LogWarning($"Failed to find skill with name: '{nameOrId}'.");
                return null;
            }
        }

        if (!_gameResources.Skills.TryGetValue(skillId, out SkillData skill))
        {
            _logger.LogWarning($"Cannot find skill with id: '{skillId}'.");
            return null;
        }

        return skill;
    }
}
