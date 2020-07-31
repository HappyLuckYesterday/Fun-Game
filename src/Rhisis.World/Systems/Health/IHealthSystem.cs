using Rhisis.Core.Data;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Systems.Health
{
    public interface IHealthSystem
    {
        int GetMaxOriginHp(ILivingEntity entity);

        int GetMaxOriginMp(ILivingEntity entity);

        int GetMaxOriginFp(ILivingEntity entity);

        int GetMaxOriginPoints(ILivingEntity entity, DefineAttributes attribute);

        int GetMaxHp(ILivingEntity entity);

        int GetMaxMp(ILivingEntity entity);

        int GetMaxFp(ILivingEntity entity);

        int GetMaxPoints(ILivingEntity entity, DefineAttributes attribute);

        int GetHpRecovery(ILivingEntity entity);

        int GetMpRecovery(ILivingEntity entity);

        int GetFpRecovery(ILivingEntity entity);

        int GetRecoveryPoints(ILivingEntity entity, DefineAttributes attribute);

        int GetPoints(ILivingEntity entity, DefineAttributes attribute);

        void SetPoints(ILivingEntity entity, DefineAttributes attribute, int value);

        void IncreasePoints(ILivingEntity entity, DefineAttributes attribute, int value);

        void IdleRecovery(IPlayerEntity player, bool isSitted = false);
    }
}
