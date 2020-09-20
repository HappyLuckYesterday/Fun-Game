using Rhisis.Game.Abstractions.Behavior;

namespace Rhisis.Game.Abstractions.Entities
{
    public interface IInteligentEntity : IWorldObject
    {
        IBehavior Behavior { get; }
    }
}
