using Microsoft.Extensions.Logging;
using Rhisis.Core.Data;
using Rhisis.Network.Packets;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Rhisis.World.Game.Structures;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Skills;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.World.Handlers
{
    [Handler]
    public class UseSkillHandler
    {
        private readonly ILogger<UseSkillHandler> _logger;
        private readonly ISkillSystem _skillSystem;
        private readonly ISkillPacketFactory _skillPacketFactory;

        public UseSkillHandler(ILogger<UseSkillHandler> logger, ISkillSystem skillSystem, ISkillPacketFactory skillPacketFactory)
        {
            _logger = logger;
            _skillSystem = skillSystem;
            _skillPacketFactory = skillPacketFactory;
        }

        /// <summary>
        /// Use a skill for a given player.
        /// </summary>
        /// <param name="serverClient">Current client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.USESKILL)]
        public void OnUseSkill(IWorldServerClient serverClient, UseSkillPacket packet)
        {
            if (packet.SkillIndex < 0 || packet.SkillIndex > (int)DefineJob.JobMax.MAX_SKILLS)
            {
                _logger.LogWarning($"Player {serverClient.Player} tried to use an unknown skill: '{packet.SkillIndex}'.");
                _skillPacketFactory.SendSkillCancellation(serverClient.Player);
                return;
            }

            if (packet.TargetObjectId < 0)
            {
                _logger.LogWarning($"Player {serverClient.Player} tried to use a skill on an unknown target.");
                _skillPacketFactory.SendSkillCancellation(serverClient.Player);
                return;
            }

            Skill skill = serverClient.Player.SkillTree.GetSkillByIndex(packet.SkillIndex);

            _skillSystem.UseSkill(serverClient.Player, skill, packet.TargetObjectId, packet.UseType);
        }
    }
}
