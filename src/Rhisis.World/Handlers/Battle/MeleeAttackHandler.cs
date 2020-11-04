﻿using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
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

            if (player.Battle.CanAttack(monster))
            {
                player.Battle.MeleeAttack(monster, packet.AttackMessage);
            }
        }
    }
}
