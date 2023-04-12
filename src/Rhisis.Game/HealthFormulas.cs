using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using System;

namespace Rhisis.Game;

public static class HealthFormulas
{
    #region Max origin

    public static int GetMaxOriginHp(Mover entity)
    {
        if (entity is Player player)
        {
            float maxHpFactor = player.Job.MaxHpFactor;
            int level = player.Level;
            int stamina = GetStatisticPoints(player, DefineAttributes.DST_STA);

            float a = (maxHpFactor * level) / 2.0f;
            float b = a * ((level + 1) / 4.0f) * (1.0f + (stamina / 50.0f)) + (stamina * 10f);

            return (int)(b + 80f);
        }
        else if (entity is Monster monster)
        {
            return monster.Properties.AddHp;
        }

        return 0;
    }

    public static int GetMaxOriginMp(Mover entity)
    {
        int level = entity.Level;
        int intelligence = GetStatisticPoints(entity, DefineAttributes.DST_INT);

        if (entity is Player player)
        {
            float maxMpFactor = player.Job.MaxMpFactor;

            return (int)((((level * 2.0f) + (intelligence * 8.0f)) * maxMpFactor) + 22.0f + (intelligence * maxMpFactor));
        }

        return (level * 2) + (intelligence * 8) + 22;
    }

    public static int GetMaxOriginFp(Mover entity)
    {
        int level = entity.Level;
        int stamina = GetStatisticPoints(entity, DefineAttributes.DST_STA);
        int dexterity = GetStatisticPoints(entity, DefineAttributes.DST_DEX);

        if (entity is Player player)
        {
            float maxFpFactor = player.Job.MaxFpFactor;

            return (int)((((level * 2.0f) + (stamina * 6.0f)) * maxFpFactor) + (stamina * maxFpFactor));
        }

        int strength = GetStatisticPoints(entity, DefineAttributes.DST_STR);

        return ((level * 2) + (strength * 7) + (stamina * 2) + (dexterity * 4));
    }

    #endregion

    #region Recovery

    public static int GetHpRecovery(Mover entity)
    {
        int level = entity.Level;
        int stamina = GetStatisticPoints(entity, DefineAttributes.DST_STA);
        int maxHp = GetMaxHp(entity);
        float hpRecoveryFactor = entity is Player player ? player.Job.HpRecoveryFactor : 1f;

        int recoveredHp = (int)((level / 3.0f) + (maxHp / (500.0f * level)) + (stamina * hpRecoveryFactor));

        return ReduceRecoveryPercent(recoveredHp);
    }

    public static int GetMpRecovery(Mover entity)
    {
        int level = entity.Level;
        int intelligence = GetStatisticPoints(entity, DefineAttributes.DST_INT);
        int maxMp = GetMaxMp(entity);
        float mpRecoveryFactor = entity is Player player ? player.Job.MpRecoveryFactor : 1f;
        int recoveredMp = (int)(((level * 1.5f) + (maxMp / (500.0f * level)) + (intelligence * mpRecoveryFactor)) * 0.2f);

        return ReduceRecoveryPercent(recoveredMp);
    }

    public static int GetFpRecovery(Mover entity)
    {
        int level = entity.Level;
        int stamina = GetStatisticPoints(entity, DefineAttributes.DST_STA);
        int maxFp = GetMaxFp(entity);
        float fpRecoveryFactor = entity is Player player ? player.Job.FpRecoveryFactor : 1f;
        int recoveredFp = (int)(((level * 2.0f) + (maxFp / (500.0f * level)) + (stamina * fpRecoveryFactor)) * 0.2f);

        return ReduceRecoveryPercent(recoveredFp);
    }

    #endregion

    #region Maximum

    public static int GetMaxHp(Mover entity)
    {
        return GetMaxParamPoints(GetMaxOriginHp(entity),
            entity.Attributes.Get(DefineAttributes.DST_HP_MAX),
            entity.Attributes.Get(DefineAttributes.DST_HP_MAX_RATE));
    }

    public static int GetMaxMp(Mover entity)
    {
        return GetMaxParamPoints(GetMaxOriginMp(entity),
            entity.Attributes.Get(DefineAttributes.DST_MP_MAX),
            entity.Attributes.Get(DefineAttributes.DST_MP_MAX_RATE));
    }

    public static int GetMaxFp(Mover entity)
    {
        return GetMaxParamPoints(GetMaxOriginFp(entity),
            entity.Attributes.Get(DefineAttributes.DST_FP_MAX),
            entity.Attributes.Get(DefineAttributes.DST_FP_MAX_RATE));
    }

    #endregion

    private static int GetStatisticPoints(Mover mover, DefineAttributes attribute)
    {
        return GetOriginalPoints(mover, attribute) + mover.Attributes.Get(attribute);
    }

    private static int GetOriginalPoints(Mover mover, DefineAttributes attribute)
    {
        return attribute switch
        {
            DefineAttributes.DST_STR => mover.Statistics?.Strength ?? default,
            DefineAttributes.DST_STA => mover.Statistics?.Stamina ?? default,
            DefineAttributes.DST_DEX => mover.Statistics?.Dexterity ?? default,
            DefineAttributes.DST_INT => mover.Statistics?.Intelligence ?? default,
            _ => default
        };
    }

    private static int GetMaxParamPoints(int originValue, int additonnal, int maxFactor)
    {
        int maxValue = originValue + additonnal;

        float factor = 1f + (maxFactor / 100f);

        maxValue = Math.Max((int)(maxValue * factor), 1);

        return maxValue;
    }

    private static int ReduceRecoveryPercent(int recovery) => (int)(recovery - (recovery * 0.1f));
}