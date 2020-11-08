using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using System.Collections;
using System.Collections.Generic;

namespace Rhisis.Game.Features
{
    public class Projectiles : GameFeature, IProjectiles
    {
        private readonly IDictionary<int, IProjectile> _projectiles;
        private readonly IMover _mover;

        public IProjectile this[int key] => _projectiles[key];

        public IEnumerable<int> Keys => _projectiles.Keys;

        public IEnumerable<IProjectile> Values => _projectiles.Values;

        public int Count => _projectiles.Count;

        public Projectiles(IMover mover)
        {
            _mover = mover;
            _projectiles = new Dictionary<int, IProjectile>();
        }

        public void Add(int projectileId, IProjectile projectile)
        {
            _projectiles.Add(projectileId, projectile);
        }

        public void Remove(int projectileId)
        {
            _projectiles.Remove(projectileId);
        }

        public bool ContainsKey(int key) => _projectiles.ContainsKey(key);

        public IEnumerator<KeyValuePair<int, IProjectile>> GetEnumerator() => _projectiles.GetEnumerator();

        public bool TryGetValue(int key, out IProjectile value) => _projectiles.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => _projectiles.GetEnumerator();
    }
}
