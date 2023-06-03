using Microsoft.Extensions.Logging;
using Rhisis.Game.Battle.Projectiles;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;

namespace Rhisis.WorldServer.Handlers.Projectiles;

[PacketHandler(PacketType.SFX_ID)]
internal sealed class ProjectileLaunchedHandler : WorldPacketHandler
{
    private readonly ILogger<ProjectileLaunchedHandler> _logger;

    public ProjectileLaunchedHandler(ILogger<ProjectileLaunchedHandler> logger)
    {
        _logger = logger;
    }

    public void Execute(SfxIdPacket packet)
    {
        Projectile projectile = Player.Projectiles.Get(packet.IdSfxHit);

        if (projectile is not null)
        {
            bool isProjectileValid = true;

            if (projectile.Target.ObjectId != packet.TargetId)
            {
                _logger.LogError($"Invalid projectile target for '{Player.Name}'");
                isProjectileValid = false;
            }

            if (projectile.Type != (AttackFlags)packet.Type)
            {
                _logger.LogError($"Invalid projectile type for '{Player.Name}'");
                isProjectileValid = false;
            }

            if (!isProjectileValid)
            {
                Player.Projectiles.Remove(packet.IdSfxHit);
            }
        }
    }
}
