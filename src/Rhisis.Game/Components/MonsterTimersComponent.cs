namespace Rhisis.Game.Abstractions.Components
{
    public class MonsterTimersComponent : IMonsterTimers
    {
        public long NextMoveTime { get; set; }

        public long NextAttackTime { get; set; }
    }
}
