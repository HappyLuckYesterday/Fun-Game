using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Common;
using Rhisis.Game.Features.AttackArbiters;
using Rhisis.Game.Protocol.Snapshots.Battle;
using System;

namespace Rhisis.Game.Features
{
    public class Battle : GameFeature, IBattle
    {
        private readonly IMover _mover;
        private readonly ILogger<Battle> _logger;

        public bool IsFighting => Target != null;

        public IMover Target { get; set; }

        public Battle(IMover mover, ILogger<Battle> logger)
        {
            _mover = mover;
            _logger = logger;
        }

        public bool CanAttack(IMover target)
        {
            if (_mover == target)
            {
                _logger.LogError($"{_mover} cannot attack itself.");
                return false;
            }

            if (_mover.Health.IsDead)
            {
                _logger.LogError($"{_mover} cannot attack because its dead.");
                return false;
            }

            if (target.Health.IsDead)
            {
                _logger.LogError($"{_mover} cannot attack {target} because target is already dead.");
                return false;
            }

            return true;
        }

        public void ClearTarget()
        {
            Target = null;
        }

        public void MeleeAttack(IMover target, ObjectMessageType objectMessageType)
        {
            AttackResult attackResult = new MeleeAttackArbiter(_mover, target).CalculateDamages();

            if (!attackResult.Flags.HasFlag(AttackFlags.AF_MISS))
            {
                // TODO: reduce damages with defense
                int damages = Math.Max(0, attackResult.Damages);

                target.Health.SufferDamages(_mover, damages, attackResult.Flags, objectMessageType);

                if (target is IMonster monster)
                {
                    if (monster.Health.IsDead)
                    {
                        ClearTarget();
                        monster.Battle.ClearTarget();
                        monster.Unfollow();
                    }
                    else
                    {
                        monster.Follow(_mover);
                        monster.Battle.Target = _mover;
                    }
                }
            }

            using var meleeAttackSnapshot = new MeleeAttackSnapshot(_mover, target, objectMessageType, attackResult.Flags);

            SendPacketToVisible(_mover, meleeAttackSnapshot);
        }

        public void RangeAttack(IMover target, int power, ObjectMessageType objectMessageType)
        {
            throw new NotImplementedException();
        }
    }
}
