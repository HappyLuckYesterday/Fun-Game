using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.IO;
using Rhisis.Game.Common;
using Rhisis.World.Game.Entities;
using Rhisis.World.Packets;
using Rhisis.World.Systems.Statistics;
using System;

namespace Rhisis.World.Systems.Health
{
    [Injectable(ServiceLifetime.Transient)]
    public class HealthSystem : IHealthSystem
    {
        private readonly IStatisticsSystem _statisticsSystem;
        private readonly IMoverPacketFactory _moverPacketFactory;

        public HealthSystem(IStatisticsSystem statisticsSystem, IMoverPacketFactory moverPacketFactory)
        {
            _statisticsSystem = statisticsSystem;
            _moverPacketFactory = moverPacketFactory;
        }

        public int GetMaxOriginHp(ILivingEntity entity)
        {
            if (entity is IPlayerEntity player)
            {
                float maxHpFactor = player.PlayerData.JobData.MaxHpFactor;
                int level = player.Object.Level;
                int stamina = _statisticsSystem.GetTotalStamina(player);

                float a = (maxHpFactor * level) / 2.0f;
                float b = a * ((level + 1) / 4.0f) * (1.0f + (stamina / 50.0f)) + (stamina * 10f);

                return (int)(b + 80f);
            }
            else if (entity is IMonsterEntity monster)
            {
                return monster.Data.AddHp;
            }

            return 0;
        }

        public int GetMaxOriginMp(ILivingEntity entity)
        {
            int level = entity.Object.Level;
            int intelligence = _statisticsSystem.GetTotalIntelligence(entity);

            if (entity is IPlayerEntity player)
            {
                float maxMpFactor = player.PlayerData.JobData.MaxMpFactor;

                return (int)((((level * 2.0f) + (intelligence * 8.0f)) * maxMpFactor) + 22.0f + (intelligence * maxMpFactor));
            }

            return (level * 2) + (intelligence * 8) + 22;
        }

        public int GetMaxOriginFp(ILivingEntity entity)
        {
            int level = entity.Object.Level;
            int stamina = _statisticsSystem.GetTotalStamina(entity);
            int dexterity = _statisticsSystem.GetTotalDexterity(entity);

            if (entity is IPlayerEntity player)
            {
                float maxFpFactor = player.PlayerData.JobData.MaxFpFactor;

                return (int)((((level * 2.0f) + (stamina * 6.0f)) * maxFpFactor) + (stamina * maxFpFactor));
            }

            int strength = _statisticsSystem.GetTotalStrength(entity);

            return ((level * 2) + (strength * 7) + (stamina * 2) + (dexterity * 4));
        }

        public int GetMaxOriginPoints(ILivingEntity entity, DefineAttributes attribute)
        {
            return attribute switch
            {
                DefineAttributes.HP => GetMaxOriginHp(entity),
                DefineAttributes.MP => GetMaxOriginMp(entity),
                DefineAttributes.FP => GetMaxOriginFp(entity),
                _ => 0
            };
        }

        public int GetMaxHp(ILivingEntity entity)
        {
            return GetMaxParamPoints(GetMaxOriginHp(entity), 
                entity.Attributes[DefineAttributes.HP_MAX], 
                entity.Attributes[DefineAttributes.HP_MAX_RATE]);
        }

        public int GetMaxMp(ILivingEntity entity)
        {
            return GetMaxParamPoints(GetMaxOriginMp(entity),
                entity.Attributes[DefineAttributes.MP_MAX],
                entity.Attributes[DefineAttributes.MP_MAX_RATE]);
        }

        public int GetMaxFp(ILivingEntity entity)
        {
            return GetMaxParamPoints(GetMaxOriginFp(entity),
                entity.Attributes[DefineAttributes.FP_MAX],
                entity.Attributes[DefineAttributes.FP_MAX_RATE]);
        }

        public int GetMaxPoints(ILivingEntity entity, DefineAttributes attribute)
        {
            return attribute switch
            {
                DefineAttributes.HP => GetMaxHp(entity),
                DefineAttributes.MP => GetMaxMp(entity),
                DefineAttributes.FP => GetMaxFp(entity),
                _ => 0
            };
        }

        public int GetHpRecovery(ILivingEntity entity)
        {
            int level = entity.Object.Level;
            int stamina = _statisticsSystem.GetTotalStamina(entity);
            int maxHp = GetMaxHp(entity);
            float hpRecoveryFactor = entity is IPlayerEntity player ? player.PlayerData.JobData.HpRecoveryFactor : 1f;

            int recoveredHp = (int)((level / 3.0f) + (maxHp / (500.0f * level)) + (stamina * hpRecoveryFactor));

            return ReduceRecoveryPercent(recoveredHp);
        }

        public int GetMpRecovery(ILivingEntity entity)
        {
            int level = entity.Object.Level;
            int intelligence = _statisticsSystem.GetTotalIntelligence(entity);
            int maxMp = GetMaxMp(entity);
            float mpRecoveryFactor = entity is IPlayerEntity player ? player.PlayerData.JobData.MpRecoveryFactor : 1f;
            int recoveredMp = (int)(((level * 1.5f) + (maxMp / (500.0f * level)) + (intelligence * mpRecoveryFactor)) * 0.2f);

            return ReduceRecoveryPercent(recoveredMp);
        }

        public int GetFpRecovery(ILivingEntity entity)
        {
            int level = entity.Object.Level;
            int stamina = _statisticsSystem.GetTotalStamina(entity);
            int maxFp = GetMaxFp(entity);
            float fpRecoveryFactor = entity is IPlayerEntity player ? player.PlayerData.JobData.FpRecoveryFactor : 1f;
            int recoveredFp = (int)(((level * 2.0f) + (maxFp / (500.0f * level)) + (stamina * fpRecoveryFactor)) * 0.2f);

            return ReduceRecoveryPercent(recoveredFp);
        }

        public int GetRecoveryPoints(ILivingEntity entity, DefineAttributes attribute)
        {
            return attribute switch
            {
                DefineAttributes.HP => GetHpRecovery(entity),
                DefineAttributes.MP => GetMpRecovery(entity),
                DefineAttributes.FP => GetFpRecovery(entity),
                _ => 0
            };
        }

        public int GetPoints(ILivingEntity entity, DefineAttributes attribute)
        {
            return attribute switch
            {
                DefineAttributes.HP => entity.Attributes[DefineAttributes.HP],
                DefineAttributes.MP => entity.Attributes[DefineAttributes.MP],
                DefineAttributes.FP => entity.Attributes[DefineAttributes.FP],
                _ => 0
            };
        }

        public void SetPoints(ILivingEntity entity, DefineAttributes attribute, int value)
        {
            if (attribute == DefineAttributes.HP || attribute == DefineAttributes.MP || attribute == DefineAttributes.FP)
            {
                int max = GetMaxPoints(entity, attribute);

                if (entity.Attributes[attribute] == value)
                {
                    return;
                }

                entity.Attributes[attribute] = Math.Min(value, max);
            }
        }

        public void IncreasePoints(ILivingEntity entity, DefineAttributes attribute, int value)
        {
            if (value == -1)
            {
                SetPoints(entity, attribute, int.MaxValue);
            }
            else
            {
                int newHp = GetPoints(entity, attribute) + value;

                SetPoints(entity, attribute, newHp);
            }
        }

        public void IdleRecovery(IPlayerEntity player, bool isSitted = false)
        {
            if (player.IsDead)
            {
                return;
            }

            const int NextIdleHealSit = 2;
            const int NextIdleHealStand = 3;
            int nextHealDelay = isSitted ? NextIdleHealSit : NextIdleHealStand;

            player.Timers.NextHealTime = Time.TimeInSeconds() + nextHealDelay;

            int recoveryHp = GetHpRecovery(player);
            int recoveryMp = GetMpRecovery(player);
            int recoveryFp = GetFpRecovery(player);

            IncreasePoints(player, DefineAttributes.HP, recoveryHp);
            IncreasePoints(player, DefineAttributes.MP, recoveryMp);
            IncreasePoints(player, DefineAttributes.FP, recoveryFp);

            _moverPacketFactory.SendUpdatePoints(player, DefineAttributes.HP, player.Attributes[DefineAttributes.HP]);
            _moverPacketFactory.SendUpdatePoints(player, DefineAttributes.MP, player.Attributes[DefineAttributes.MP]);
            _moverPacketFactory.SendUpdatePoints(player, DefineAttributes.FP, player.Attributes[DefineAttributes.FP]);
        }

        private int ReduceRecoveryPercent(int recovery) => (int)(recovery - (recovery * 0.1f));

        private int GetMaxParamPoints(int originValue, int additonnal, int maxFactor)
        {
            int maxValue = originValue + additonnal;

            float factor = 1f + (maxFactor / 100f);

            maxValue = Math.Max((int)(maxValue * factor), 1);

            return maxValue;
        }
    }
}
