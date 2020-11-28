using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Snapshots;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;
using System;

namespace Rhisis.WorldServer.Handlers
{
    [Handler]
    public class RevivalToLodestarHandler
    {
        private readonly ILogger<RevivalToLodestarHandler> _logger;
        private readonly IMapManager _mapManager;

        public RevivalToLodestarHandler(ILogger<RevivalToLodestarHandler> logger, IMapManager mapManager)
        {
            _logger = logger;
            _mapManager = mapManager;
        }

        [HandlerAction(PacketType.REVIVAL_TO_LODESTAR)]
        public void OnRevivalToLodestar(IPlayer player, INetPacketStream _)
        {
            if (!player.Health.IsDead)
            {
                _logger.LogWarning($"Player '{player.Name}' tried to revival to lodestar without being dead.");
                return;
            }

            player.Experience.ApplyDeathPenality(true);
            player.Health.ApplyDeathRecovery(true);

            IMapRevivalRegion revivalRegion = player.Map.GetNearRevivalRegion(player.Position);

            if (revivalRegion == null)
            {
                throw new InvalidOperationException("Cannot find nearest revival region.");
            }

            if (player.Map.Id != revivalRegion.MapId)
            {
                IMap revivalMap = _mapManager.GetMap(revivalRegion.MapId);

                if (revivalMap == null)
                {
                    throw new InvalidOperationException($"Failed to find map with id: {revivalMap.Id}'.");
                }

                revivalRegion = revivalMap.GetRevivalRegion(revivalRegion.Key);
            }

            player.Teleport(revivalRegion.RevivalPosition, revivalRegion.MapId);

            using var snapshots = new FFSnapshot(
                new MotionSnapshot(player, ObjectMessageType.OBJMSG_ACC_STOP | ObjectMessageType.OBJMSG_STOP_TURN | ObjectMessageType.OBJMSG_STAND),
                new RevivalToLodestarSnapshot(player));

            player.Send(snapshots);
            player.SendToVisible(snapshots);
        }
    }
}
