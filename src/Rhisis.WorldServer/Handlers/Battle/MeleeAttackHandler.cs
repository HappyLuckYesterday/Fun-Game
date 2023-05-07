using Rhisis.Game;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;

namespace Rhisis.WorldServer.Handlers.Battle;

[PacketHandler(PacketType.MELEE_ATTACK)]
internal sealed class MeleeAttackHandler : WorldPacketHandler
{
    public void Execute(MeleeAttackPacket packet)
    {
        Mover target = Player.GetVisibleObject<Mover>(packet.ObjectId) ?? throw new ArgumentException($"Cannot find target with id: '{packet.ObjectId}'");
        Item weapon = Player.Inventory.GetEquipedItem(ItemPartType.RightWeapon);

        if (weapon is not null && weapon.Properties.AttackSpeed != packet.WeaponAttackSpeed)
        {
            throw new InvalidOperationException($"Player '{Player.Name}' has a different weapon speed that the server.");
        }

        AttackType attackType = packet.AttackMessage switch
        {
            ObjectMessageType.OBJMSG_ATK1 => AttackType.MeleeAttack1,
            ObjectMessageType.OBJMSG_ATK2 => AttackType.MeleeAttack2,
            ObjectMessageType.OBJMSG_ATK3 => AttackType.MeleeAttack3,
            ObjectMessageType.OBJMSG_ATK4 => AttackType.MeleeAttack4,
            _ => throw new InvalidOperationException($"the object message type {packet.AttackMessage} can not be used during a melee attack packet")
        };

        Player.TryMeleeAttack(target, attackType);
    }
}
