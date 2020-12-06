using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;

namespace Rhisis.WorldServer.Handlers
{
    [Handler]
    public class DoUseSkillPointsHandler
    {
        private readonly ISpecialEffectSystem _specialEffectSystem;

        public DoUseSkillPointsHandler(ISpecialEffectSystem specialEffectSystem)
        {
            _specialEffectSystem = specialEffectSystem;
        }

        /// <summary>
        /// Updates the player's skill levels.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.DOUSESKILLPOINT)]
        public void OnDoUseSkillPoints(IPlayer player, DoUseSkillPointsPacket packet)
        {
            if (!packet.Skills.Any())
            {
                throw new InvalidOperationException($"Player {player} tried to update skills, but no skills were sent.");
            }

            player.SkillTree.Update(packet.Skills);
            _specialEffectSystem.StartSpecialEffect(player, DefineSpecialEffects.XI_SYS_EXCHAN01, false);
        }
    }
}
