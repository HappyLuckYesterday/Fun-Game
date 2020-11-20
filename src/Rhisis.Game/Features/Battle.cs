using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Abstractions.Features.Battle;
using Rhisis.Game.Common;
using Rhisis.Game.Features.AttackArbiters;
using Rhisis.Game.Features.AttackArbiters.Reducers;
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

        public bool TryMeleeAttack(IMover target, AttackType attackType)
        {
            if (!CanAttack(target))
            {
                return false;
            }
            if (!attackType.IsMeleeAttack())
            {
                throw new InvalidOperationException($"can not do a melee attack with attack type {attackType}");
            }

            AttackResult attackResult;

            if (!TryInflictDamagesIfOneHitKillMode(target, attackType, out attackResult))
            {
                attackResult = new MeleeAttackArbiter(_mover, target).CalculateDamages();

                if (!attackResult.Flags.HasFlag(AttackFlags.AF_MISS))
                {
                    attackResult = new MeleeAttackReducer(_mover, target).ReduceDamages(attackResult);

                    InflictDamages(_mover, target, attackResult, attackType);
                }
            }

            using var meleeAttackSnapshot = new MeleeAttackSnapshot(_mover, target, attackType, attackResult.Flags);

            SendPacketToVisible(_mover, meleeAttackSnapshot);
            return true;
        }

        public bool TryRangeAttack(IMover target, int power, AttackType rangeAttackType)
        {
            if (!CanAttack(target))
            {
                return false;
            }

            if (!rangeAttackType.IsRangeAttack())
            {
                throw new NotSupportedException($"can not cause a range attack for attack type {rangeAttackType}");
            }

            IProjectile projectile = null;

            if (rangeAttackType.CausesArrowProjectile()) 
            { 
                projectile = new ArrowProjectile(_mover, target, power, () =>
                {
                    if (TryInflictDamagesIfOneHitKillMode(target, rangeAttackType))
                    {
                        return;
                    }

                    AttackResult attackResult = new MeleeAttackArbiter(_mover, target, AttackFlags.AF_GENERIC | AttackFlags.AF_RANGE, power).CalculateDamages();

                    if (!attackResult.Flags.HasFlag(AttackFlags.AF_MISS))
                    {
                        attackResult = new MeleeAttackReducer(_mover, target).ReduceDamages(attackResult);

                        InflictDamages(_mover, target, attackResult, rangeAttackType);
                    }
                });
            }
            else if (rangeAttackType.CausesMagicProjectile())
            {
                projectile = new MagicProjectile(_mover, target, power, () =>
                {
                    if (TryInflictDamagesIfOneHitKillMode(target, rangeAttackType))
                    {
                        return;
                    }

                    AttackResult attackResult = new MagicAttackArbiter(_mover, target, power).CalculateDamages();

                    if (!attackResult.Flags.HasFlag(AttackFlags.AF_MISS))
                    {
                        InflictDamages(_mover, target, attackResult, rangeAttackType);
                    }
                });
            }

            if (projectile != null)
            {
                int projectileId = _mover.Projectiles.Add(projectile);

                using var snapshot = new RangeAttackSnapshot(_mover, rangeAttackType, target.Id, power, projectileId);
                SendPacketToVisible(_mover, snapshot);
            }

            return true;
        }

        public bool TrySkillAttack(IMover target, ISkill skill)
        {
            if (!CanAttack(target))
            {
                return false;
            }

            var skillAttackType = skill.Data.Type.ToAttackType();

            if (!skillAttackType.IsSkillAttack())
            {
                throw new NotSupportedException($"can not cause a skill attack for attack type {skillAttackType}");
            }

            if (TryInflictDamagesIfOneHitKillMode(target, skillAttackType))
            {
                return true;
            }

            AttackResult attackResult;

            if (skillAttackType.CausesMeleeSkill())
            {
                attackResult = new MeleeSkillAttackArbiter(_mover, target, skill).CalculateDamages();

                if (!attackResult.Flags.HasFlag(AttackFlags.AF_MISS))
                {
                    attackResult = new MeleeSkillAttackReducer(_mover, target, skill).ReduceDamages(attackResult);
                }
            }
            else if (skillAttackType.CausesMagicSkill())
            {
                attackResult = new MagicSkillAttackArbiter(_mover, target, skill).CalculateDamages();

                if (!attackResult.Flags.HasFlag(AttackFlags.AF_MISS))
                {
                    attackResult = new MagicSkillAttackReducer(_mover, target, skill).ReduceDamages(attackResult);
                }
            }
            else
            {
                throw new InvalidOperationException($"the attack type {skillAttackType} did not cause any type of skill");
            }

            InflictDamages(_mover, target, attackResult, skillAttackType);
            return true;
        }

        private void InflictDamages(IMover attacker, IMover target, AttackResult attackResult, AttackType attackType)
        {
            Target = target;
            target.Health.SufferDamages(attacker, Math.Max(0, attackResult.Damages), attackType, attackResult.Flags);

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

        private bool TryInflictDamagesIfOneHitKillMode(IMover target, AttackType attackType)
        {
            return TryInflictDamagesIfOneHitKillMode(target, attackType, out var _);
        }

        private bool TryInflictDamagesIfOneHitKillMode(IMover target, AttackType attackType, out AttackResult attackResult)
        {
            if (_mover is IPlayer player && player.Mode.HasFlag(ModeType.ONEKILL_MODE))
            {
                attackResult = new AttackResult()
                {
                    Damages = target.Health.Hp,
                    Flags = AttackFlags.AF_GENERIC
                };

                InflictDamages(_mover, target, attackResult, attackType);
                return true;
            }
            attackResult = null;
            return false;
        }
    }
}
