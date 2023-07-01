using Rhisis.Game;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;

namespace Rhisis.WorldServer.Handlers.Battle;

[PacketHandler(PacketType.RANGE_ATTACK)]
internal sealed class RangeAttackHandler : WorldPacketHandler
{
    public void Execute(RangeAttackPacket packet)
    {
        if (packet.Power < 0)
        {
            throw new InvalidOperationException($"Range attack power cannot be less than 0.");
        }

        if (packet.ProjectileId < 0)
        {
            throw new InvalidOperationException($"Invalid projectile id.");
        }

        Item equipedItem = Player.Inventory.GetEquipedItem(ItemPartType.RightWeapon);

        if (equipedItem is null || equipedItem.Properties.WeaponType != WeaponType.RANGE_BOW)
        {
            throw new InvalidOperationException("Cannot process a ranged attack. Player is not using a bow.");
        }

        Item bulletItem = Player.Inventory.GetEquipedItem(ItemPartType.Bullet);

        if (bulletItem is null || bulletItem.Properties.ItemKind3 != ItemKind3.ARROW)
        {
            return;
        }

        Monster target = Player.GetVisibleObject<Monster>(packet.ObjectId)
            ?? throw new InvalidOperationException($"Cannot find target object with id: '{packet.ObjectId}'.");

        Player.StopMoving();

        if (Player.TryRangeAttack(target, packet.Power, AttackType.RangeBowAttack))
        {
            Player.Inventory.DeleteItem(bulletItem, 1);
        }
    }
}
