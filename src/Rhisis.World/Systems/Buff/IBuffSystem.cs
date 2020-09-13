using Rhisis.World.Game.Entities;

namespace Rhisis.World.Systems.Buff
{
    public interface IBuffSystem
    {
        bool AddBuff(ILivingEntity entity, Game.Structures.Buff buff);

        bool RemoveBuff(ILivingEntity entity, Game.Structures.Buff buff);

        void UpdateBuffTimers(ILivingEntity entity);
    }
}
