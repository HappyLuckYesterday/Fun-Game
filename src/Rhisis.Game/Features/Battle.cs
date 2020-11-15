using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
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

        public void MeleeAttack(IMover target, ObjectMessageType objectMessageType)
        {
            AttackResult attackResult;

            if (!TryInflictDamagesIfOneHitKillMode(target, objectMessageType, out attackResult))
            {
                attackResult = new MeleeAttackArbiter(_mover, target).CalculateDamages();

                if (!attackResult.Flags.HasFlag(AttackFlags.AF_MISS))
                {
                    attackResult = new MeleeAttackReducer(_mover, target).ReduceDamages(attackResult);

                    InflictDamages(_mover, target, attackResult, objectMessageType);
                }
            }

            using var meleeAttackSnapshot = new MeleeAttackSnapshot(_mover, target, objectMessageType, attackResult.Flags);

            SendPacketToVisible(_mover, meleeAttackSnapshot);
        }

        public void RangeAttack(IMover target, int power, ObjectMessageType objectMessageType)
        {
            IProjectile projectile = null;

            if (objectMessageType.HasFlag(ObjectMessageType.OBJMSG_ATK_RANGE1))
            {
                projectile = new ArrowProjectile(_mover, target, power, () =>
                {
                    if (TryInflictDamagesIfOneHitKillMode(target, objectMessageType))
                    {
                        return;
                    }

                    AttackResult attackResult = new MeleeAttackArbiter(_mover, target, AttackFlags.AF_GENERIC | AttackFlags.AF_RANGE, power).CalculateDamages();

                    if (!attackResult.Flags.HasFlag(AttackFlags.AF_MISS))
                    {
                        attackResult = new MeleeAttackReducer(_mover, target).ReduceDamages(attackResult);

                        InflictDamages(_mover, target, attackResult, objectMessageType);
                    }
                });
            }
            else if (objectMessageType.HasFlag(ObjectMessageType.OBJMSG_ATK_MAGIC1))
            {
                projectile = new MagicProjectile(_mover, target, power, () =>
                {
                    if (TryInflictDamagesIfOneHitKillMode(target, objectMessageType))
                    {
                        return;
                    }

                    AttackResult attackResult = new MagicAttackArbiter(_mover, target, power).CalculateDamages();

                    if (!attackResult.Flags.HasFlag(AttackFlags.AF_MISS))
                    {
                        InflictDamages(_mover, target, attackResult, objectMessageType);
                    }
                });
            }

            if (projectile != null)
            {
                int projectileId = _mover.Projectiles.Add(projectile);

                using var snapshot = new RangeAttackSnapshot(_mover, objectMessageType, target.Id, power, projectileId);
                SendPacketToVisible(_mover, snapshot);
            }
        }

        public void SkillAttack(IMover target, ISkill skill)
        {
            var skillMessageType = skill.Data.Type switch
            {
                SkillType.Magic => ObjectMessageType.OBJMSG_MAGICSKILL,
                SkillType.Skill => ObjectMessageType.OBJMSG_MELEESKILL,
                _ => ObjectMessageType.OBJMSG_MELEESKILL
            };

            if (TryInflictDamagesIfOneHitKillMode(target, skillMessageType))
            {
                return;
            }

            AttackResult attackResult;

            if (skillMessageType == ObjectMessageType.OBJMSG_MELEESKILL)
            {
                attackResult = new MeleeSkillAttackArbiter(_mover, target, skill).CalculateDamages();

                if (!attackResult.Flags.HasFlag(AttackFlags.AF_MISS))
                {
                    attackResult = new MeleeSkillAttackReducer(_mover, target, skill).ReduceDamages(attackResult);
                }
            }
            else
            {
                attackResult = new MagicSkillAttackArbiter(_mover, target, skill).CalculateDamages();

                if (!attackResult.Flags.HasFlag(AttackFlags.AF_MISS))
                {
                    attackResult = new MagicSkillAttackReducer(_mover, target, skill).ReduceDamages(attackResult);
                }
            }

            InflictDamages(_mover, target, attackResult, skillMessageType);            
        }

        private void InflictDamages(IMover attacker, IMover target, AttackResult attackResult, ObjectMessageType objectMessageType)
        {
            Target = target;
            target.Health.SufferDamages(attacker, Math.Max(0, attackResult.Damages), attackResult.Flags, objectMessageType);

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

        private bool TryInflictDamagesIfOneHitKillMode(IMover target, ObjectMessageType objectMessageType)
        {
            return TryInflictDamagesIfOneHitKillMode(target, objectMessageType, out var _);
        }

        private bool TryInflictDamagesIfOneHitKillMode(IMover target, ObjectMessageType objectMessageType, out AttackResult attackResult)
        {
            if (_mover is IPlayer player && player.Mode.HasFlag(ModeType.ONEKILL_MODE))
            {
                attackResult = new AttackResult()
                {
                    Damages = target.Health.Hp,
                    Flags = AttackFlags.AF_GENERIC
                };

                InflictDamages(_mover, target, attackResult, objectMessageType);
                return true;
            }
            attackResult = null;
            return false;
        }
    }
}
