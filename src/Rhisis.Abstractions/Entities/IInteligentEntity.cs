using Rhisis.Abstractions.Behavior;

namespace Rhisis.Abstractions.Entities
{
    /// <summary>
    /// Describes the behavior of an inteligent entity.
    /// </summary>
    public interface IInteligentEntity : IWorldObject
    {
        /// <summary>
        /// Gets the entity's behavior.
        /// </summary>
        IBehavior Behavior { get; }
    }
}
