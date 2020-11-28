using Microsoft.Extensions.Logging;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Common;
using System;

namespace Rhisis.WorldServer.Game.Chat
{
    [ChatCommand("/createmonster", AuthorityType.Administrator)]
    [ChatCommand("/cn", AuthorityType.Administrator)]
    [ChatCommand("/monster", AuthorityType.Administrator)]
    public class CreateMonsterChatCommand : IChatCommand
    {
        private readonly ILogger<CreateMonsterChatCommand> _logger;
        private readonly IMapManager _mapManager;

        /// <summary>
        /// Creates a new <see cref="CreateMonsterChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="mapManager"></param>

        public CreateMonsterChatCommand(ILogger<CreateMonsterChatCommand> logger, IMapManager mapManager)
        {
            _logger = logger;
            _mapManager = mapManager;
        }

        /// <inheritdoc />
        public void Execute(IPlayer player, object[] parameters)
        {
            if (parameters.Length <= 0 || parameters.Length > 2)
            {
                throw new ArgumentException($"Create monster command must have 1 or 2 parameters.", nameof(parameters));
            }

            if (!int.TryParse((string)parameters[0], out int monsterId))
            {
                throw new ArgumentException($"Cannot convert the first parameter in int.", nameof(parameters));
            }

            int quantityToSpawn = 1;

            if (parameters.Length == 2)
            {
                if (!int.TryParse((string)parameters[1], out quantityToSpawn))
                {
                    throw new ArgumentException($"Cannot convert the second parameter in int.", nameof(parameters));
                }
            }
            _logger.LogTrace($"{player.Name} want to spawn {quantityToSpawn} mmonster");

            throw new NotImplementedException();
            //const int sizeOfSpawnArea = 13;
            //const int respawnTime = 65535;

            //IMapInstance currentMap = player.CurrentMap;
            //IMapLayer currentMapLayer = currentMap.GetMapLayer(player.Object.LayerId);
            //Vector3 currentPosition = player.Object.Position.Clone();
            //var respawnRegion = new MapRespawnRegion((int)currentPosition.X - sizeOfSpawnArea / 2, (int)currentPosition.Z - sizeOfSpawnArea / 2, sizeOfSpawnArea, sizeOfSpawnArea, respawnTime, 0, WorldObjectType.Mover, monsterId, quantityToSpawn);
            
            //for (int i = 0; i < quantityToSpawn; i++)
            //{
            //    IMonsterEntity monsterToCreate = _monsterFactory.CreateMonster(currentMap, currentMapLayer, monsterId, respawnRegion, true);
            //    monsterToCreate.Object.Position = player.Position.Clone();

            //    currentMapLayer.AddEntity(monsterToCreate);
            //}
        }
    }
}