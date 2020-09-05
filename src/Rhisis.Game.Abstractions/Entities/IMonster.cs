namespace Rhisis.Game.Abstractions.Entities
{
    public interface IMonster : IMover
    {
        bool IsAggresive { get; }
    }
}
