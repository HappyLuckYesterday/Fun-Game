using Rhisis.Abstractions.Entities;

namespace Rhisis.Abstractions.Systems;

/// <summary>
/// Provides a mechanism to manage the world entity visibility.
/// </summary>
public interface IVisibilitySystem
{
    /// <summary>
    /// Executes the visibility system.
    /// </summary>
    /// <param name="mover">Mover entity.</param>
    void Execute(IMover mover);
}
