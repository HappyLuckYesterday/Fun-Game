namespace Rhisis.Game.Abstractions.Components
{
    public interface IMonsterTimers
    {
        long NextMoveTime { get; set; }

        long NextAttackTime { get; set; }

        long DespawnTime { get; set; }

        long RespawnTime { get; set; }
    }
}
