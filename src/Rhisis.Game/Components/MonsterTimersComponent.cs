using Rhisis.Game.Abstractions.Features;

namespace Rhisis.Game.Abstractions.Components
{
    public class MonsterTimersComponent : IMonsterTimers
    {
        public long NextMoveTime { get; set; }

        public long NextAttackTime { get; set; }

        public long DespawnTime { get; set; }

        public long RespawnTime { get; set; }
    }
}
