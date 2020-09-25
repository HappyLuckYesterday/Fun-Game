using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;

namespace Rhisis.World.Handlers.Battle
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

            if (!(target is IMonster monster))
            {
                throw new InvalidOperationException($"Target '{target.Name}' is not a monster.");
            }

            // TODO: check weapon attack speed
            // TODO: call battle system

            //Item weaponItem = serverClient.Player.Inventory.GetEquipedItem(ItemPartType.RightWeapon) ?? serverClient.Player.Hand;

            //if (weaponItem != null && weaponItem.Data?.AttackSpeed != packet.WeaponAttackSpeed)
            //{
            //    _logger.LogCritical($"Player {serverClient.Player.Object.Name} has changed his weapon speed.");
            //    return;
            //}

            //_battleSystem.MeleeAttack(serverClient.Player, target, packet.AttackMessage, packet.WeaponAttackSpeed);
        }
    }
}
