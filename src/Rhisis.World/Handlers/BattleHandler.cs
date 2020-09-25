﻿using Microsoft.Extensions.Logging;
using Rhisis.Game.Common;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Systems.Battle;
using Sylver.HandlerInvoker.Attributes;
using System;

namespace Rhisis.World.Handlers
{
    [Handler]
    public class BattleHandler
    {
        private readonly ILogger<BattleHandler> _logger;
        private readonly IBattleSystem _battleSystem;

        /// <summary>
        /// Creates a new <see cref="BattleHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="battleSystem">Battle system.</param>
        public BattleHandler(ILogger<BattleHandler> logger, IBattleSystem battleSystem)
        {
            _logger = logger;
            _battleSystem = battleSystem;
        }

        /// <summary>
        /// On magic attack.
        /// </summary>
        /// <param name="serverClient">Current client.</param>
        /// <param name="packet">Magic attack incoming packet.</param>
        //[HandlerAction(PacketType.MAGIC_ATTACK)]
        public void OnMagicAttack(IWorldServerClient serverClient, MagicAttackPacket packet)
        {
            var target = serverClient.Player.FindEntity<IMonsterEntity>(packet.TargetObjectId);

            if (target == null)
            {
                _logger.LogError($"Cannot find target with object id {packet.TargetObjectId}");
                return;
            }

            if (packet.MagicPower < 0)
            {
                _logger.LogWarning($"Magic attack power cannot be less than 0.");
            }

            if (packet.ProjectileId < 0)
            {
                _logger.LogError($"{serverClient.Player} Invalid projectile id.");
                return;
            }

            _battleSystem.MagicAttack(serverClient.Player, target, packet.AttackMessage, Math.Max(0, packet.MagicPower), packet.ProjectileId + 1);
        }

        /// <summary>
        /// On ranged attack.
        /// </summary>
        /// <param name="serverClient">Current client.</param>
        /// <param name="packet">Range attack incoming packet.</param>
        //[HandlerAction(PacketType.RANGE_ATTACK)]
        public void OnRangeAttack(IWorldServerClient serverClient, RangeAttackPacket packet)
        {
            var target = serverClient.Player.FindEntity<IMonsterEntity>(packet.ObjectId);

            if (target == null)
            {
                _logger.LogError($"Cannot find target with object id {packet.ObjectId}");
                return;
            }

            if (packet.Power < 0)
            {
                _logger.LogWarning($"Range attack power cannot be less than 0.");
            }

            if (packet.ProjectileId < 0)
            {
                _logger.LogError($"{serverClient.Player} Invalid projectile id.");
                return;
            }

            _battleSystem.RangeAttack(serverClient.Player, target, packet.AttackMessage, Math.Max(0, packet.Power), packet.ProjectileId + 1);
        }
    }
}
