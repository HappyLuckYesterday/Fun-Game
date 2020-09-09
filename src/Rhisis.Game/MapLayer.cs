using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using System;
using System.Collections.Generic;

namespace Rhisis.Game
{
    public class MapLayer : IMapLayer
    {
        private readonly List<IPlayer> _players;
        private readonly List<IMonster> _monsters;

        public IEnumerable<IPlayer> Players => _players;

        public IEnumerable<IMonster> Monsters => _monsters;

        public IMap ParentMap { get; }

        public MapLayer(IMap map)
        {
            ParentMap = map;
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
            throw new NotImplementedException();
        }
    }
}
