using Rhisis.Core.Helpers;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Resources;
using Rhisis.Game.Resources.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game;

public sealed class MapLayer : IDisposable
{
    private static readonly int VisibilityRange = 75; // TODO: make a configuration for this

    private readonly Map _parentMap;
    private readonly List<Player> _players = new();
    private readonly List<Npc> _npcs = new();
    private readonly List<Monster> _monsters = new();

    /// <summary>
    /// Gets the layer id.
    /// </summary>
    public int Id { get; }

    public MapLayer(Map parentMap, int layerId)
    {
        _parentMap = parentMap;
        Id = layerId;

        IEnumerable<Npc> npcs = parentMap.Properties.Objects
            .Select(x => new
            {
                NpcObject = x,
                Properties = GameResources.Current.Npcs.Get(x.Name)
            })
            .Where(x => x is not null && x.Properties is not null)
            .Select(x => new Npc(x.Properties)
            {
                Map = parentMap,
                MapLayer = this,
                Position = x.NpcObject.Position.Clone(),
                RotationAngle = x.NpcObject.Angle,
                ModelId = x.NpcObject.ModelId,
                IsSpawned = true,
                ObjectState = ObjectState.OBJSTA_STAND
            })
            .ToList();

        IEnumerable<Monster> monsters = parentMap.Properties.Regions
            .OfType<MapRespawnRegionProperties>()
            .Where(x => x.ObjectType == WorldObjectType.Mover)
            .SelectMany(x =>
            {
                Rectangle region = new(x.X, x.Z, x.Width, x.Length);
                MoverProperties moverProperties = GameResources.Current.Movers.Get(x.ModelId);

                return Enumerable.Range(0, x.Count).Select(i => new Monster(moverProperties)
                {
                    Name = moverProperties.Name,
                    Level = moverProperties.Level,
                    Size = moverProperties.Class == MoverClassType.RANK_BOSS ? (short)200 : (short)100,
                    ModelId = moverProperties.Id,
                    RespawnTime = x.Time,
                    Region = region,
                    Position = region.GetRandomPosition(x.Height),
                    RotationAngle = FFRandom.FloatRandom(0, 360),
                    IsSpawned = true,
                    ObjectState = ObjectState.OBJSTA_STAND,
                    Map = parentMap,
                    MapLayer = this
                }).ToList();
            })
            .ToList();

        _npcs.AddRange(npcs);
        _monsters.AddRange(monsters);
    }

    /// <summary>
    /// Add a player to the current map layer.
    /// </summary>
    /// <param name="player">Player to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when the given player instance is null.</exception>
    public void AddPlayer(Player player)
    {
        if (player is null)
        {
            throw new ArgumentNullException(nameof(player), "Cannot add a undefined player instance.");
        }

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

    /// <summary>
    /// Removes a player from the current map layer.
    /// </summary>
    /// <param name="player">Player to remove.</param>
    /// <exception cref="ArgumentNullException">Thrown when the given player instance is null.</exception>
    public void RemovePlayer(Player player)
    {
        if (player is null)
        {
            throw new ArgumentNullException(nameof(player), "Cannot remove a undefined player instance.");
        }

        if (_players.Contains(player))
        {
            lock (_players)
            {
                if (_players.Contains(player))
                {
                    _players.Remove(player);
                }
            }
        }
    }

    /// <summary>
    /// Updates the map layer logic.
    /// </summary>
    public void Update()
    {
        lock (_players)
        {
            if (!_players.Any())
            {
                return;
            }

            foreach (Player player in _players)
            {
                player.Update();
            }
        }

        lock (_monsters)
        {
            if (!_monsters.Any())
            {
                return;
            }

            foreach (Monster monster in _monsters)
            {
                monster.Update();
            }
        }
    }

    /// <summary>
    /// Updates the map layer logic every seconds.
    /// </summary>
    public void UpdateSeconds()
    {
        lock (_npcs)
        {
            if (!_npcs.Any())
            {
                return;
            }

            foreach (Npc npc in _npcs)
            {
                npc.Update();
            }
        }
    }

    public void Dispose()
    {
        lock (_players)
        {
            _players.Clear();
        }
    }

    public IEnumerable<WorldObject> GetVisibleObjects(WorldObject worldObject)
    {
        var objects = new List<WorldObject>();

        lock (_players)
        {
            objects.AddRange(GetVisibleObjects(worldObject, _players));
        }

        lock (_npcs)
        {
            objects.AddRange(GetVisibleObjects(worldObject, _npcs));
        }

        lock (_monsters)
        {
            objects.AddRange(GetVisibleObjects(worldObject, _monsters));
        }

        return objects;
    }

    private static IEnumerable<TObjects> GetVisibleObjects<TObjects>(WorldObject worldObject, IEnumerable<TObjects> objects)
        where TObjects : WorldObject
    {
        return objects.Where(x => x.ObjectId != worldObject.ObjectId && x.IsSpawned && x.IsVisible && x.Position.IsInRange(worldObject.Position, VisibilityRange));
    }
}