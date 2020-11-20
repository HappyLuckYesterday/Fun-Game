using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.IO;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using Rhisis.Game.Abstractions.Features.Battle;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Snapshots;
using System;

namespace Rhisis.Game.Abstractions.Components
{
    public class Health : GameFeature, IHealth
    {
        private readonly IMover _mover;
        private readonly IGameResources _gameResources;
        private readonly Lazy<IHealthFormulas> _healthFormulas;

        private int _hp;
        private int _mp;
        private int _fp;
        private long _nextHealTime;

        public bool IsDead => Hp <= 0;

        public int Hp
        {
            get => _hp;
            set
            {
                if (_hp == value)
                {
                    return;
                }

                _hp = Math.Clamp(value, 0, MaxHp);
            }
        }

        public int Mp
        {
            get => _mp;
            set
            {
                if (_mp == value)
                {
                    return;
                }

                _mp = Math.Clamp(value, 0, MaxMp);
            }
        }

        public int Fp
        {
            get => _fp;
            set
            {
                if (_fp == value)
                {
                    return;
                }

                _fp = Math.Clamp(value, 0, MaxFp);
            }
        }

        public int MaxHp => _healthFormulas.Value.GetMaxHp(_mover);

        public int MaxMp => _healthFormulas.Value.GetMaxMp(_mover);

        public int MaxFp => _healthFormulas.Value.GetMaxFp(_mover);

        /// <summary>
        /// Creates a new <see cref="Health"/> instance.
        /// </summary>
        /// <param name="mover">Current mover.</param>
        /// <param name="gameResources">Game resources.</param>
        public Health(IMover mover, IGameResources gameResources)
        {
            _mover = mover;
            _gameResources = gameResources;
            _nextHealTime = Time.TimeInSeconds();
            _healthFormulas = new Lazy<IHealthFormulas>(() => mover.Systems.GetService<IHealthFormulas>());
        }

        public void RegenerateAll()
        {
            Hp = MaxHp;
            Mp = MaxMp;
            Fp = MaxFp;

            SendHealth();
        }

        public void Die(IMover killer, AttackType attackType, bool sendHitPoints = false)
        {
            Hp = 0;

            if (_mover is IPlayer && killer is IPlayer)
            {
                // TODO: PVP
            }
            else
            {
                using var moverDeathSnapshot = new FFSnapshot();
                moverDeathSnapshot.Merge(new MoverDeathSnapshot(_mover, killer, attackType));

                if (sendHitPoints)
                {
                    moverDeathSnapshot.Merge(new UpdateParamPointSnapshot(_mover, DefineAttributes.HP, Hp));
                }

                SendPacketToVisible(_mover, moverDeathSnapshot, sendToPlayer: true);
            }

            _mover.Behavior.OnKilled(killer);
            killer.Behavior.OnTargetKilled(_mover);
        }

        public void SufferDamages(IMover attacker, int damages, AttackType attackType, AttackFlags attackFlags = AttackFlags.AF_GENERIC)
        {
            int damagesToInflict = Math.Min(Hp, damages);

            using var damageSnapshots = new FFSnapshot();

            damageSnapshots.Merge(new AddDamageSnapshot(_mover, attacker, attackFlags, damagesToInflict));

            if (damagesToInflict > 0)
            {
                Hp -= damagesToInflict;
                damageSnapshots.Merge(new UpdateParamPointSnapshot(_mover, DefineAttributes.HP, Hp));
            }

            SendPacketToVisible(_mover, damageSnapshots, sendToPlayer: true);

            if (IsDead)
            {
                Die(attacker, attackType, sendHitPoints: true);
            }
        }

        public void IdleHeal()
        {
            if (IsDead || _nextHealTime > Time.TimeInSeconds())
            {
                return;
            }

            // TODO: next heal time to configuration file?
            const int NextIdleHealSit = 2;
            const int NextIdleHealStand = 3;
            _nextHealTime = Time.TimeInSeconds() + (_mover.ObjectState == ObjectState.OBJSTA_SIT ? NextIdleHealSit : NextIdleHealStand);

            Hp += _healthFormulas.Value.GetHpRecovery(_mover);
            Mp += _healthFormulas.Value.GetMpRecovery(_mover);
            Fp += _healthFormulas.Value.GetFpRecovery(_mover);

            SendHealth();
        }

        public void ApplyDeathRecovery(bool send = false)
        {
            if (!IsDead || !(_mover is IPlayer))
            {
                return;
            }

            decimal recoveryRate = _gameResources.Penalities.GetRevivalPenality(_mover.Level) / 100;

            Hp = (int)(MaxHp * recoveryRate);
            Mp = (int)(MaxMp * recoveryRate);
            Fp = (int)(MaxFp * recoveryRate);

            if (send)
            {
                SendHealth();
            }
        }

        public int GetCurrent(DefineAttributes attribute)
        {
            return attribute switch
            {
                DefineAttributes.HP => Hp,
                DefineAttributes.MP => Mp,
                DefineAttributes.FP => Fp,
                _ => -1
            };
        }

        public void SetCurrent(DefineAttributes attribute, int value, bool send = true)
        {
            switch (attribute)
            {
                case DefineAttributes.HP:
                    Hp = value;
                    break;
                case DefineAttributes.MP:
                    Mp = value;
                    break;
                case DefineAttributes.FP:
                    Fp = value;
                    break;
            }

            if (send)
            {
                using var healthSnapshot = new UpdateParamPointSnapshot(_mover, attribute, GetCurrent(attribute));

                SendPacketToVisible(_mover, healthSnapshot, sendToPlayer: true);
            }
        }

        public int GetMaximum(DefineAttributes attribute)
        {
            return attribute switch
            {
                DefineAttributes.HP => MaxHp,
                DefineAttributes.MP => MaxMp,
                DefineAttributes.FP => MaxFp,
                _ => -1
            };
        }

        private void SendHealth()
        {
            using var healthSnapshot = new FFSnapshot();
            healthSnapshot.Merge(new UpdateParamPointSnapshot(_mover, DefineAttributes.HP, Hp));
            healthSnapshot.Merge(new UpdateParamPointSnapshot(_mover, DefineAttributes.MP, Mp));
            healthSnapshot.Merge(new UpdateParamPointSnapshot(_mover, DefineAttributes.FP, Fp));

            SendPacketToVisible(_mover, healthSnapshot, sendToPlayer: true);
        }
    }
}
