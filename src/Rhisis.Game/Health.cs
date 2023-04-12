using Rhisis.Core.IO;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Protocol;
using System;

namespace Rhisis.Game;

public sealed class Health
{
    private readonly Mover _mover;

    private int _hp;
    private int _mp;
    private int _fp;
    private long _nextHealTime;

    /// <summary>
    /// Gets or sets the health points.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the Mana points.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the Fatigue points.
    /// </summary>
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

    /// <summary>
    /// Gets the maximum Health points for the current mover.
    /// </summary>
    public int MaxHp => HealthFormulas.GetMaxHp(_mover);

    /// <summary>
    /// Gets the maximum Mana points for the current mover.
    /// </summary>
    public int MaxMp => HealthFormulas.GetMaxMp(_mover);

    /// <summary>
    /// Gets the maximum Fatigue points for the current mover.
    /// </summary>
    public int MaxFp => HealthFormulas.GetMaxFp(_mover);

    /// <summary>
    /// Creates a new <see cref="Health"/> instance.
    /// </summary>
    /// <param name="mover">Current mover.</param>
    public Health(Mover mover)
    {
        _mover = mover;
        _nextHealTime = Time.TimeInSeconds();
    }

    public void RegenerateAll()
    {
        Hp = MaxHp;
        Mp = MaxMp;
        Fp = MaxFp;

        SendHealth();
    }

    public void Die(Mover killer, AttackType attackType, bool sendHitPoints = false)
    {
        Hp = 0;

        if (_mover is Player && killer is Player)
        {
            // TODO: PVP
        }
        else
        {
            using FFSnapshot moverDeathSnapshot = new();
            moverDeathSnapshot.Merge(new MoverDeathSnapshot(_mover, killer, attackType));

            if (sendHitPoints)
            {
                moverDeathSnapshot.Merge(new UpdateParamPointSnapshot(_mover, DefineAttributes.DST_HP, Hp));
            }

            _mover.SendToVisible(moverDeathSnapshot, sendToSelf: true);
        }

        //_mover.Behavior.OnKilled(killer);
        //killer.Behavior.OnTargetKilled(_mover);
    }

    public void SufferDamages(Mover attacker, int damages, AttackType attackType, AttackFlags attackFlags = AttackFlags.AF_GENERIC)
    {
        int damagesToInflict = Math.Min(Hp, damages);

        using FFSnapshot damageSnapshots = new();

        damageSnapshots.Merge(new AddDamageSnapshot(_mover, attacker, attackFlags, damagesToInflict));

        if (damagesToInflict > 0)
        {
            Hp -= damagesToInflict;
            damageSnapshots.Merge(new UpdateParamPointSnapshot(_mover, DefineAttributes.DST_HP, Hp));
        }

        _mover.SendToVisible(damageSnapshots, sendToSelf: true);

        if (Hp <= 0)
        {
            Die(attacker, attackType, sendHitPoints: true);
        }
    }

    public void IdleHeal()
    {
        if (Hp <= 0 || _nextHealTime > Time.TimeInSeconds())
        {
            return;
        }

        // TODO: next heal time to configuration file?
        const int NextIdleHealSit = 2;
        const int NextIdleHealStand = 3;
        _nextHealTime = Time.TimeInSeconds() + (_mover.ObjectState == ObjectState.OBJSTA_SIT ? NextIdleHealSit : NextIdleHealStand);

        Hp += HealthFormulas.GetHpRecovery(_mover);
        Mp += HealthFormulas.GetMpRecovery(_mover);
        Fp += HealthFormulas.GetFpRecovery(_mover);

        SendHealth();
    }

    public void ApplyDeathRecovery(bool send = false)
    {
        if (Hp > 0 || _mover is not Player)
        {
            return;
        }

        decimal recoveryRate = 0;//_gameResources.Penalities.GetRevivalPenality(_mover.Level) / 100;

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
            DefineAttributes.DST_HP => Hp,
            DefineAttributes.DST_MP => Mp,
            DefineAttributes.DST_FP => Fp,
            _ => -1
        };
    }

    public void SetCurrent(DefineAttributes attribute, int value, bool send = true)
    {
        switch (attribute)
        {
            case DefineAttributes.DST_HP:
                Hp = value;
                break;
            case DefineAttributes.DST_MP:
                Mp = value;
                break;
            case DefineAttributes.DST_FP:
                Fp = value;
                break;
        }

        if (send)
        {
            using UpdateParamPointSnapshot healthSnapshot = new(_mover, attribute, GetCurrent(attribute));

            _mover.SendToVisible(healthSnapshot, sendToSelf: true);
        }
    }

    public int GetMaximum(DefineAttributes attribute)
    {
        return attribute switch
        {
            DefineAttributes.DST_HP => MaxHp,
            DefineAttributes.DST_MP => MaxMp,
            DefineAttributes.DST_FP => MaxFp,
            _ => -1
        };
    }

    private void SendHealth()
    {
        using FFSnapshot healthSnapshot = new();
        healthSnapshot.Merge(new UpdateParamPointSnapshot(_mover, DefineAttributes.DST_HP, Hp));
        healthSnapshot.Merge(new UpdateParamPointSnapshot(_mover, DefineAttributes.DST_MP, Mp));
        healthSnapshot.Merge(new UpdateParamPointSnapshot(_mover, DefineAttributes.DST_FP, Fp));

        _mover.SendToVisible(healthSnapshot, sendToSelf: true);
    }
}
