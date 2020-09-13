using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Packets.Internal
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class SkillPacketFactory : PacketFactoryBase, ISkillPacketFactory
    {
        /// <inheritdoc />
        public void SendSkillTreeUpdate(IPlayerEntity player)
        {
            using var packet = new FFPacket();

            packet.StartNewMergedPacket(player.Id, SnapshotType.DOUSESKILLPOINT);
            player.SkillTree.Serialize(packet);
            packet.Write((int)player.SkillTree.SkillPoints);

            SendToPlayer(player, packet);
        }

        /// <inheritdoc />
        public void SendUseSkill(ILivingEntity player, IWorldEntity target, Skill skill, int castingTime, SkillUseType skillUseType)
        {
            using var packet = new FFPacket();

            packet.StartNewMergedPacket(player.Id, SnapshotType.USESKILL);
            packet.Write(skill.SkillId);
            packet.Write(skill.Level);
            packet.Write(target.Id);
            packet.Write((int)skillUseType);
            packet.Write(castingTime);

            SendToVisible(packet, player, sendToPlayer: true);
        }

        public void SendSkillCancellation(IPlayerEntity player)
        {
            using var packet = new FFPacket();

            packet.StartNewMergedPacket(player.Id, SnapshotType.CLEAR_USESKILL);

            SendToPlayer(player, packet);
        }

        public void SendSkillReset(IPlayerEntity player, ushort skillPoints)
        {
            using var packet = new FFPacket();

            packet.StartNewMergedPacket(player.Id, SnapshotType.INITSKILLPOINT);
            packet.Write((int)skillPoints);

            SendToPlayer(player, packet);
        }

        public void SendSkillState(ILivingEntity entity, int skillId, int skillLevel, int time)
        {
            using var packet = new FFPacket();

            packet.StartNewMergedPacket(entity.Id, SnapshotType.SETSKILLSTATE);
            packet.Write(entity.Id);
            packet.Write((short)BuffType.Skill);
            packet.Write((short)skillId);
            packet.Write(skillLevel);
            packet.Write(time);

            SendToVisible(packet, entity, sendToPlayer: true);
        }
    }
}