using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions.Systems
{
    public interface IHealthFormulas
    {
        int GetMaxOriginHp(IMover entity);

        int GetMaxOriginMp(IMover entity);

        int GetMaxOriginFp(IMover entity);

        int GetMaxHp(IMover entity);

        int GetMaxMp(IMover entity);

        int GetMaxFp(IMover entity);

        int GetHpRecovery(IMover entity);

        int GetMpRecovery(IMover entity);

        int GetFpRecovery(IMover entity);
    }
}
