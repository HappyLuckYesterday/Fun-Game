using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Snapshots.Skills;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;

namespace Rhisis.WorldServer.Handlers
{
    [Handler]
    public class UseSkillHandler
    {
        /// <summary>
        /// Use a skill for a given player.
        /// </summary>
        /// <param name="serverClient">Current client.</param>
        /// <param name="packet">Incoming packet.</param>
        [HandlerAction(PacketType.USESKILL)]
        public void OnUseSkill(IPlayer player, UseSkillPacket packet)
        {
            if (packet.SkillIndex < 0 || packet.SkillIndex > (int)DefineJob.JobMax.MAX_SKILLS)
            {
                CancelSkill(player);
                throw new InvalidOperationException($"Attempt to use an unknown skill: '{packet.SkillIndex}'.");
            }

            if (packet.TargetObjectId < 0)
            {
                CancelSkill(player);
                throw new InvalidOperationException($"Attempt to use a skill on an unknown target: '{packet.TargetObjectId}'.");
            }

            if (!player.SkillTree.TryGetSkillAtIndex(packet.SkillIndex, out ISkill skill))
            {
                CancelSkill(player);
                throw new InvalidOperationException($"Failed to find skill at index: '{packet.SkillIndex}'.");
            }

            IMover target = player.Id == packet.TargetObjectId
                ? player
                : player.VisibleObjects.OfType<IMover>().FirstOrDefault(x => x.Id == packet.TargetObjectId);

            if (target == null)
            {
                throw new InvalidOperationException($"Cannot find target with id: '{packet.TargetObjectId}'.");
            }

            if (skill.CanUse(target))
            {
                try
                {
                    skill.Use(target);
                }
                catch
                {
                    CancelSkill(player);
                    throw;
                }
            }
            else
            {
                CancelSkill(player);
            }
        }

        private void CancelSkill(IPlayer player)
        {
            using var cancelSkillSnapshot = new ClearUseSkillSnapshot(player);

            player.Send(cancelSkillSnapshot);
            player.SendToVisible(cancelSkillSnapshot);
        }
    }
}
