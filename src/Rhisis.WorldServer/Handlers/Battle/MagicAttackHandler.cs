using Rhisis.Game;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;

namespace Rhisis.WorldServer.Handlers.Battle;

[PacketHandler(PacketType.MAGIC_ATTACK)]
internal sealed class MagicAttackHandler : WorldPacketHandler
{
    public void Execute(MagicAttackPacket packet)
    {
        Player.StopMoving();

        if (packet.MagicPower < 0)
        {
            throw new InvalidOperationException($"Range attack power cannot be less than 0.");
        }

        if (packet.ProjectileId < 0)
        {
            throw new InvalidOperationException($"Invalid projectile id.");
        }

        Item weapon = Player.Inventory.GetEquipedItem(ItemPartType.LeftWeapon) ?? throw new InvalidOperationException($"Cannot cast magic attack: no weapon.");

        if (weapon.Properties.WeaponType is not (WeaponType.MAGIC_WAND or WeaponType.MELEE_STAFF))
        {
            throw new InvalidOperationException("Cannot cast magic attack: invalid weapon.");
        }
        
        Monster target = Player.GetVisibleObject<Monster>(packet.TargetObjectId)
            ?? throw new InvalidOperationException($"Cannot find target object with id: '{packet.TargetObjectId}'.");

        if (Player.TryRangeAttack(target, packet.MagicPower, AttackType.RangeWandAttack))
        {
            if (packet.MagicPower > 3)
            {
                Player.Health.Mp -= (weapon.Properties.RequiredMp * Player.Attributes.Get(DefineAttributes.DST_MP_DEC_RATE) / 100);
            }
        }
    }
}
