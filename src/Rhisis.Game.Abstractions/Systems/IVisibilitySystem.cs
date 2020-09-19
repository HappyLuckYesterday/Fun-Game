using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions.Systems
{
    public interface IVisibilitySystem
    {
        /// <summary>
        /// Executes the visibility system.
        /// </summary>
        /// <param name="mover">Mover entity.</param>
        void Execute(IMover mover);
    }
}
