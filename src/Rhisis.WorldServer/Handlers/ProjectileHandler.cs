using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Sylver.HandlerInvoker.Attributes;
using System.Collections.Generic;

namespace Rhisis.WorldServer.Handlers
{
    [Handler]
    public class ProjectileHandler
    {
        private readonly ILogger<ProjectileHandler> _logger;

        /// <summary>
        /// Creates a new <see cref="ProjectileHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public ProjectileHandler(ILogger<ProjectileHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Indicates that a projectile has been fired.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="packet">Projectile packet.</param>
        [HandlerAction(PacketType.SFX_ID)]
        public void OnProjectileLaunched(IPlayer player, SfxIdPacket packet)
        {
            int projectileId = packet.IdSfxHit;
            IProjectile projectile = player.Projectiles.GetValueOrDefault(projectileId);

            if (projectile != null)
            {
                bool isProjectileValid = true;

                if (projectile.Target.Id != packet.TargetId)
                {
                    _logger.LogError($"Invalid projectile target for '{player}'");
                    isProjectileValid = false;
                }

                if (projectile.Type != (AttackFlags)packet.Type)
                {
                    _logger.LogError($"Invalid projectile type for '{player}'");
                    isProjectileValid = false;
                }

                if (!isProjectileValid)
                {
                    player.Projectiles.Remove(projectileId);
                }
            }
        }

        /// <summary>
        /// Indicates that a projectile has arrivied and hit its target.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="packet">Projectile hit packet.</param>
        [HandlerAction(PacketType.SFX_HIT)]
        public void OnProjectileArrived(IPlayer player, SfxHitPacket packet)
        {
            int projectileId = packet.Id;
            IProjectile projectile = player.Projectiles.GetValueOrDefault(projectileId);

            if (projectile != null)
            {
                bool isProjectileValid = packet.AttackerId == player.Id;

                if (projectile.Type == AttackFlags.AF_MAGIC && projectile is IMagicProjectile magicProjectile)
                {
                    isProjectileValid = isProjectileValid && packet.MagicPower == magicProjectile.MagicPower;
                }
                else if (projectile.Type == AttackFlags.AF_MAGICSKILL && projectile is IMagicSkillProjectile magicSkillProjectile)
                {
                    isProjectileValid = isProjectileValid && packet.SkillId == magicSkillProjectile.Skill.Id;
                }
                else if (projectile.Type.HasFlag(AttackFlags.AF_RANGE) && projectile is IArrowProjectile arrowProjectile)
                {
                    isProjectileValid = isProjectileValid && packet.MagicPower == arrowProjectile.Power;
                }

                if (isProjectileValid)
                {
                    projectile.OnArrived?.Invoke();
                }
                else
                {
                    _logger.LogError($"Invalid projectile information for player '{player}'.");
                }

                player.Projectiles.Remove(projectileId);
            }
            else
            {
                _logger.LogError($"Cannot find projectile with id '{projectileId}' for '{player}'.");
            }
        }
    }
}
