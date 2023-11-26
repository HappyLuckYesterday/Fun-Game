using Microsoft.Extensions.Logging;
using Rhisis.Game.Battle.Projectiles;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.WorldServer.Handlers.Projectiles;

[PacketHandler(PacketType.SFX_HIT)]
internal sealed class ProjectileArrivedHandler : WorldPacketHandler
{
    private readonly ILogger<ProjectileArrivedHandler> _logger;

    public ProjectileArrivedHandler(ILogger<ProjectileArrivedHandler> logger)
    {
        _logger = logger;
    }

    public void Execute(SfxHitPacket packet)
    {
        Projectile projectile = Player.Projectiles.Get(packet.Id);

        if (projectile is not null)
        {
            bool isProjectileValid = packet.AttackerId == Player.ObjectId;

            if (projectile.Type == AttackFlags.AF_MAGIC && projectile is MagicProjectile magicProjectile)
            {
                isProjectileValid = isProjectileValid && packet.MagicPower == magicProjectile.MagicPower;
            }
            else if (projectile.Type == AttackFlags.AF_MAGICSKILL && projectile is MagicSkillProjectile magicSkillProjectile)
            {
                isProjectileValid = isProjectileValid && packet.SkillId == magicSkillProjectile.Skill.Id;
            }
            else if (projectile.Type.HasFlag(AttackFlags.AF_RANGE) && projectile is ArrowProjectile arrowProjectile)
            {
                isProjectileValid = isProjectileValid && packet.MagicPower == arrowProjectile.Power;
            }

            if (isProjectileValid)
            {
                projectile.OnArrived?.Invoke();
            }
            else
            {
                _logger.LogError($"Invalid projectile information for player '{Player.Name}'.");
            }

            Player.Projectiles.Remove(packet.Id);
        }
    }
}
