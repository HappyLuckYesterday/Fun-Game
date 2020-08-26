namespace Rhisis.Game.Entities
{
    public interface IMonster : IMover
    {
        bool IsAggresive { get; }
    }
}
