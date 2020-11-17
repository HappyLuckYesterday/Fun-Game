using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Battle;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;

namespace Rhisis.World.Handlers.Battle
{
    [Handler]
    public class RangeAttackHandler
    {
        [HandlerAction(PacketType.RANGE_ATTACK)]
        public void Execute(IPlayer player, RangeAttackPacket packet)
        {
            var target = player.VisibleObjects.OfType<IMonster>().FirstOrDefault(x => x.Id == packet.ObjectId);

            if (target == null)
            {
                throw new InvalidOperationException($"Cannot find target with object id {packet.ObjectId}");
            }

            if (packet.Power < 0)
            {
                throw new InvalidOperationException($"Range attack power cannot be less than 0.");
            }

            if (packet.ProjectileId < 0)
            {
                throw new InvalidOperationException($"Invalid projectile id.");
            }

            IItem equipedItem = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon);

            if (equipedItem == null || equipedItem.Data.WeaponType != WeaponType.RANGE_BOW)
            {
                throw new InvalidOperationException("Cannot process a ranged attack. Player is not using a bow.");
            }

            IItem bulletItem = player.Inventory.GetEquipedItem(ItemPartType.Bullet);

            if (bulletItem == null || bulletItem.Data.ItemKind3 != ItemKind3.ARROW)
            {
                return;
            }

            if(player.Battle.TryRangeAttack(target, Math.Max(0, packet.Power), AttackType.RangeBowAttack))
            {
                player.Inventory.DeleteItem(bulletItem, 1);
            }
        }
    }
}
