﻿using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Skills;
using Sylver.HandlerInvoker.Attributes;
using System.Linq;

namespace Rhisis.World.Handlers
{
    [Handler]
    public class SkillHandler
    {
        private readonly ILogger<SkillHandler> _logger;
        private readonly ISkillSystem _skillSystem;
        private readonly ISkillPacketFactory _skillPacketFactory;

        public SkillHandler(ILogger<SkillHandler> logger, ISkillSystem skillSystem, ISkillPacketFactory skillPacketFactory)
        {
            _logger = logger;
            _skillSystem = skillSystem;
            _skillPacketFactory = skillPacketFactory;
        }

        /// <summary>
        /// Updates the player's skill levels.
        /// </summary>
        /// <param name="client">Current client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.DOUSESKILLPOINT)]
        public void OnDoUseSkillPoints(IWorldClient client, DoUseSkillPointsPacket packet)
        {
            if (!packet.Skills.Any())
            {
                _logger.LogWarning($"Player {client.Player} tried to update skills, but no skills were sent.");
                return;
            }

            _skillSystem.UpdateSkills(client.Player, packet.Skills);
        }

        /// <summary>
        /// Use a skill for a given player.
        /// </summary>
        /// <param name="client">Current client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.USESKILL)]
        public void OnUseSkill(IWorldClient client, UseSkillPacket packet)
        {
            if (packet.SkillIndex < 0 || packet.SkillIndex > (int)DefineJob.JobMax.MAX_SKILLS)
            {
                _logger.LogWarning($"Player {client.Player} tried to use an unknown skill: '{packet.SkillIndex}'.");
                _skillPacketFactory.SendSkillCancellation(client.Player);
                return;
            }

            if (packet.TargetObjectId < 0)
            {
                _logger.LogWarning($"Player {client.Player} tried to use a skill on an unknown target.");
                _skillPacketFactory.SendSkillCancellation(client.Player);
                return;
            }

            SkillInfo skill = client.Player.SkillTree.GetSkillByIndex(packet.SkillIndex);

            _skillSystem.UseSkill(client.Player, skill, packet.TargetObjectId, packet.UseType);
        }

        [HandlerAction(PacketType.SFX_ID)]
        public void OnProjectileLaunched(IWorldClient client, SfxIdPacket packet)
        {
            // TODO
        }

        [HandlerAction(PacketType.SFX_HIT)]
        public void OnProjectileArrived(IWorldClient client, SfxHitPacket packet)
        {
            // TODO
        }
    }
}
