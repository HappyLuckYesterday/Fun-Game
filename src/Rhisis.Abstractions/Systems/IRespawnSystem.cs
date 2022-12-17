using Rhisis.Abstractions.Entities;

namespace Rhisis.Abstractions.Systems;

/// <summary>
/// Provides a mechanism to manage the world object's respawn.
/// </summary>
public interface IRespawnSystem
{
    /// <summary>
    /// Process the given world object respawn.
    /// </summary>
    /// <param name="worldObject"></param>
    void Execute(IWorldObject worldObject);
}
