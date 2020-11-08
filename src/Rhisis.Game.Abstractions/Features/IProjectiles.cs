using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Features
{
    public interface IProjectiles : IReadOnlyDictionary<int, IProjectile>
    {
        void Add(int projectileId, IProjectile projectile);

        void Remove(int projectileId);
    }
}
