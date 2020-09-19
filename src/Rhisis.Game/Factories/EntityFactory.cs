using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Behavior;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Factories;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using Rhisis.Game.Entities;

namespace Rhisis.Game.Factories
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class EntityFactory : IEntityFactory
    {
        private readonly ILogger<EntityFactory> _logger;
        private readonly IGameResources _gameResources;
        private readonly IMapManager _mapManager;
        private readonly IBehaviorManager _behaviorManager;

        public EntityFactory(ILogger<EntityFactory> logger, IGameResources gameResources, IMapManager mapManager, IBehaviorManager behaviorManager)
        {
            _logger = logger;
            _gameResources = gameResources;
            _mapManager = mapManager;
            _behaviorManager = behaviorManager;
        }

        public IMapItem CreateMapItem()
        {
            return null;
        }

        public IMonster CreateMonster(int moverId, int mapId, int mapLayerId, Vector3 position, IMapRespawnRegion respawnRegion)
        {
            if (!_gameResources.Movers.TryGetValue(moverId, out MoverData moverData))
            {
                _logger.LogError($"Cannot find mover data for mover: '{moverId}'.");
                return null;
            }

            IMap map = _mapManager.GetMap(mapId);

            if (map == null)
            {
                _logger.LogError($"Cannot find map with id: '{mapId}'.");
                return null;
            }

            var monster = new Monster
            {
                Data = moverData,
                Map = map,
                MapLayer = map.GetMapLayer(mapLayerId),
                Position = position.Clone(),
                RespawnRegion = respawnRegion,
                ObjectState = ObjectState.OBJSTA_STAND,
                Angle = RandomHelper.FloatRandom(0, 360f),
                Size = GameConstants.DefaultObjectSize,
                Spawned = true
            };
            monster.Health.Hp = moverData.AddHp;
            monster.Health.Mp = moverData.AddMp;
            monster.Statistics.Strength = moverData.Strength;
            monster.Statistics.Stamina = moverData.Stamina;
            monster.Statistics.Dexterity = moverData.Dexterity;
            monster.Statistics.Intelligence = moverData.Intelligence;
            monster.Behavior = _behaviorManager.GetDefaultBehavior(BehaviorType.Monster, monster);

            if (monster.Data.Class == MoverClassType.RANK_BOSS)
            {
                monster.Size *= 2;
            }

            return monster;
        }

        public INpc CreateNpc()
        {
            return null;
        }
    }
}
