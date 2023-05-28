using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;

namespace Rhisis.WorldServer.Handlers.Skills;

[PacketHandler(PacketType.USESKILL)]
internal sealed class UseSkillHanler : WorldPacketHandler
{
    public void Execute(UseSkillPacket packet)
    {
        if (packet.SkillIndex < 0 || packet.SkillIndex > (int)DefineJob.JobMax.MAX_SKILLS)
        {
            Player.CancelSkillUsage();
            throw new InvalidOperationException($"Attempt to use an unknown skill: '{packet.SkillIndex}'.");
        }

        if (packet.TargetObjectId < 0)
        {
            Player.CancelSkillUsage();
            throw new InvalidOperationException($"Attempt to use a skill on an unknown target: '{packet.TargetObjectId}'.");
        }

        if (!Player.Skills.TryGetSkillAtIndex(packet.SkillIndex, out Skill skill))
        {
            Player.CancelSkillUsage();
            throw new InvalidOperationException($"Failed to find skill at index: '{packet.SkillIndex}'.");
        }

        Mover target = Player.ObjectId == packet.TargetObjectId ? Player : Player.GetVisibleObject<Mover>(packet.TargetObjectId);

        if (target is null)
        {
            throw new InvalidOperationException($"Failed to find target with id: {packet.TargetObjectId}.");
        }

        if (skill.CanUse(target))
        {
            try
            {
                skill.Use(target);
            }
            catch
            {
                Player.CancelSkillUsage();
            }
        }
        else
        {
            Player.CancelSkillUsage();
        }
    }
}