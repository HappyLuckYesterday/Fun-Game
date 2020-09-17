using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Map;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Map
{
    public class MapLayer : IMapLayer
    {
        private readonly List<IPlayer> _players;
        private readonly List<IMonster> _monsters;
        
        public int Id { get; }

        public IMap ParentMap { get; }

        public IEnumerable<IPlayer> Players => _players;

        public IEnumerable<IMonster> Monsters => _monsters;

        public MapLayer(IMap map, int id)
        {
            ParentMap = map;
            Id = id;
            _players = new List<IPlayer>();
            _monsters = new List<IMonster>();
        }

        public void AddMonster(IMonster monster)
        {
            if (!_monsters.Contains(monster))
            {
                lock (_monsters)
                {
                    if (!_monsters.Contains(monster))
                    {
                        _monsters.Add(monster);
                    }
                }
            }
        }

        public void AddPlayer(IPlayer player)
        {
            if (!_players.Contains(player))
            {
                lock (_players)
                {
                    if (!_players.Contains(player))
                    {
                        _players.Add(player);
                    }
                }
            }
        }

        public void RemoveMonster(IMonster monster)
        {
            throw new NotImplementedException();
        }

        public void RemovePlayer(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void Process()
        {
            if (_players.Any())
            {
                UpdateObjects(_players);
                UpdateObjects(_monsters);
            }
        }

        private void UpdateObjects(IEnumerable<IWorldObject> objects)
        {
            lock (objects)
            {
                if (objects.Any())
                {
                    foreach (IWorldObject worldObject in objects)
                    {
                        // TODO: update objects
                    }
                }
            }
        }
    }
}
