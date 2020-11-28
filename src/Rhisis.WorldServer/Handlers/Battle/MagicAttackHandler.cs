using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Battle;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;

namespace Rhisis.WorldServer.Handlers.Battle
{
    [Handler]
    public class MagicAttackHandler
    {
        [HandlerAction(PacketType.MAGIC_ATTACK)]
        public void Execute(IPlayer player, MagicAttackPacket packet)
        {
            var target = player.VisibleObjects.OfType<IMonster>().FirstOrDefault(x => x.Id == packet.TargetObjectId);

            if (target == null)
            {
                throw new InvalidOperationException($"Cannot find target with object id {packet.TargetObjectId}");
            }

            if (packet.MagicPower < 0)
            {
                throw new InvalidOperationException($"Range attack power cannot be less than 0.");
            }

            if (packet.ProjectileId < 0)
            {
                throw new InvalidOperationException($"Invalid projectile id.");
            }

            player.Battle.TryRangeAttack(target, Math.Max(0, packet.MagicPower), AttackType.RangeWandAttack);
        }
    }
}
