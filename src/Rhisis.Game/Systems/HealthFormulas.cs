using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions.Components;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Systems;
using Rhisis.Game.Common;
using System;

namespace Rhisis.Game.Systems
{
    [Injectable]
    public class HealthFormulas : IHealthFormulas
    {
        #region Max origin

        public int GetMaxOriginHp(IMover entity)
        {
            if (entity is IPlayer player)
            {
                float maxHpFactor = player.Job.MaxHpFactor;
                int level = player.Level;
                int stamina = GetStatisticPoints(player, DefineAttributes.STA);

                float a = (maxHpFactor * level) / 2.0f;
                float b = a * ((level + 1) / 4.0f) * (1.0f + (stamina / 50.0f)) + (stamina * 10f);

                return (int)(b + 80f);
            }
            else if (entity is IMonster monster)
            {
                return monster.Data.AddHp;
            }

            return 0;
        }

        public int GetMaxOriginMp(IMover entity)
        {
            int level = entity.Level;
            int intelligence = GetStatisticPoints(entity, DefineAttributes.INT);

            if (entity is IPlayer player)
            {
                float maxMpFactor = player.Job.MaxMpFactor;

                return (int)((((level * 2.0f) + (intelligence * 8.0f)) * maxMpFactor) + 22.0f + (intelligence * maxMpFactor));
            }

            return (level * 2) + (intelligence * 8) + 22;
        }

        public int GetMaxOriginFp(IMover entity)
        {
            int level = entity.Level;
            int stamina = GetStatisticPoints(entity, DefineAttributes.STA);
            int dexterity = GetStatisticPoints(entity, DefineAttributes.DEX);

            if (entity is IPlayer player)
            {
                float maxFpFactor = player.Job.MaxFpFactor;

                return (int)((((level * 2.0f) + (stamina * 6.0f)) * maxFpFactor) + (stamina * maxFpFactor));
            }

            int strength = GetStatisticPoints(entity, DefineAttributes.STR);

            return ((level * 2) + (strength * 7) + (stamina * 2) + (dexterity * 4));
        }

        #endregion

        #region Recovery

        public int GetHpRecovery(IMover entity)
        {
            int level = entity.Level;
            int stamina = GetStatisticPoints(entity, DefineAttributes.STA);
            int maxHp = GetMaxHp(entity);
            float hpRecoveryFactor = entity is IPlayer player ? player.Job.HpRecoveryFactor : 1f;

            int recoveredHp = (int)((level / 3.0f) + (maxHp / (500.0f * level)) + (stamina * hpRecoveryFactor));

            return ReduceRecoveryPercent(recoveredHp);
        }

        public int GetMpRecovery(IMover entity)
        {
            int level = entity.Level;
            int intelligence = GetStatisticPoints(entity, DefineAttributes.INT);
            int maxMp = GetMaxMp(entity);
            float mpRecoveryFactor = entity is IPlayer player ? player.Job.MpRecoveryFactor : 1f;
            int recoveredMp = (int)(((level * 1.5f) + (maxMp / (500.0f * level)) + (intelligence * mpRecoveryFactor)) * 0.2f);

            return ReduceRecoveryPercent(recoveredMp);
        }
        
        public int GetFpRecovery(IMover entity)
        {
            int level = entity.Level;
            int stamina = GetStatisticPoints(entity, DefineAttributes.STA);
            int maxFp = GetMaxFp(entity);
            float fpRecoveryFactor = entity is IPlayer player ? player.Job.FpRecoveryFactor : 1f;
            int recoveredFp = (int)(((level * 2.0f) + (maxFp / (500.0f * level)) + (stamina * fpRecoveryFactor)) * 0.2f);

            return ReduceRecoveryPercent(recoveredFp);
        }

        #endregion

        #region Maximum

        public int GetMaxHp(IMover entity)
        {
            return GetMaxParamPoints(GetMaxOriginHp(entity),
                entity.Attributes.Get(DefineAttributes.HP_MAX),
                entity.Attributes.Get(DefineAttributes.HP_MAX_RATE));
        }

        public int GetMaxMp(IMover entity)
        {
            return GetMaxParamPoints(GetMaxOriginMp(entity),
                entity.Attributes.Get(DefineAttributes.MP_MAX),
                entity.Attributes.Get(DefineAttributes.MP_MAX_RATE));
        }

        public int GetMaxFp(IMover entity)
        {
            return GetMaxParamPoints(GetMaxOriginFp(entity),
                entity.Attributes.Get(DefineAttributes.FP_MAX),
                entity.Attributes.Get(DefineAttributes.FP_MAX_RATE));
        }

        #endregion

        private int GetStatisticPoints(IMover mover, DefineAttributes attribute)
        {
            return GetOriginalPoints(mover, attribute) + mover.Attributes.Get(attribute);
        }

        private int GetOriginalPoints(IMover mover, DefineAttributes attribute)
        {
            return attribute switch
            {
                DefineAttributes.STR => mover.Statistics?.Strength ?? default,
                DefineAttributes.STA => mover.Statistics?.Stamina ?? default,
                DefineAttributes.DEX => mover.Statistics?.Dexterity ?? default,
                DefineAttributes.INT => mover.Statistics?.Intelligence ?? default,
                _ => default
            };
        }

        private int GetMaxParamPoints(int originValue, int additonnal, int maxFactor)
        {
            int maxValue = originValue + additonnal;

            float factor = 1f + (maxFactor / 100f);

            maxValue = Math.Max((int)(maxValue * factor), 1);

            return maxValue;
        }

        private int ReduceRecoveryPercent(int recovery) => (int)(recovery - (recovery * 0.1f));
    }
}
