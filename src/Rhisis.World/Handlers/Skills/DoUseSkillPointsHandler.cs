using Microsoft.Extensions.Logging;
using Rhisis.Game.Common;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Rhisis.World.Systems.Skills;
using Rhisis.World.Systems.SpecialEffect;
using Sylver.HandlerInvoker.Attributes;
using System.Linq;

namespace Rhisis.World.Handlers
{
    [Handler]
    public class DoUseSkillPointsHandler
    {
        private readonly ILogger<UseSkillHandler> _logger;
        private readonly ISkillSystem _skillSystem;
        private readonly ISpecialEffectSystem _specialEffectSystem;

        public DoUseSkillPointsHandler(ILogger<UseSkillHandler> logger, ISkillSystem skillSystem, ISpecialEffectSystem specialEffectSystem)
        {
            _logger = logger;
            _skillSystem = skillSystem;
            _specialEffectSystem = specialEffectSystem;
        }

        /// <summary>
        /// Updates the player's skill levels.
        /// </summary>
        /// <param name="serverClient">Current client.</param>
        /// <param name="packet">Incoming packet.</param>
        //[HandlerAction(PacketType.DOUSESKILLPOINT)]
        public void OnDoUseSkillPoints(IWorldServerClient serverClient, DoUseSkillPointsPacket packet)
        {
            if (!packet.Skills.Any())
            {
                _logger.LogWarning($"Player {serverClient.Player} tried to update skills, but no skills were sent.");
                return;
            }

            if (_skillSystem.UpdateSkills(serverClient.Player, packet.Skills))
            {
                _specialEffectSystem.StartSpecialEffect(serverClient.Player, DefineSpecialEffects.XI_SYS_EXCHAN01, false);
            }
        }
    }
}
