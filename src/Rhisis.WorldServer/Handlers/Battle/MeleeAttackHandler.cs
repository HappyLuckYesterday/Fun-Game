using Rhisis.Game.Abstractions;
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
    public class MeleeAttackHandler
    {
        [HandlerAction(PacketType.MELEE_ATTACK)]
        public void Execute(IPlayer player, MeleeAttackPacket packet)
        {
            IWorldObject target = player.VisibleObjects.FirstOrDefault(x => x.Id == packet.ObjectId);

            if (target is null)
            {
                throw new InvalidOperationException($"Cannot find target with id: '{packet.ObjectId}'");
            }

            // TODO: PVP
            if (!(target is IMonster monster))
            {
                throw new InvalidOperationException($"Target '{target.Name}' is not a monster.");
            }

            IItem weapon = player.Inventory.GetEquipedItem(ItemPartType.RightWeapon) ?? player.Inventory.Hand;

            if (weapon != null && weapon.Data.AttackSpeed != packet.WeaponAttackSpeed)
            {
                throw new InvalidOperationException($"Player '{player}' has a different weapon speed that the server.");
            }

            var attackType = packet.AttackMessage switch
            {
                ObjectMessageType.OBJMSG_ATK1 => AttackType.MeleeAttack1,
                ObjectMessageType.OBJMSG_ATK2 => AttackType.MeleeAttack2,
                ObjectMessageType.OBJMSG_ATK3 => AttackType.MeleeAttack3,
                ObjectMessageType.OBJMSG_ATK4 => AttackType.MeleeAttack4,
                _ => throw new InvalidOperationException($"the object message type {packet.AttackMessage} can not be used during a melee attack packet")
            };


            player.Battle.TryMeleeAttack(monster, attackType);            
        }
    }
}
