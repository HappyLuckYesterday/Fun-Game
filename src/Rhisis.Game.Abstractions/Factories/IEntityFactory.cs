using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Entities;

namespace Rhisis.Game.Abstractions.Factories
{
    public interface IEntityFactory
    {
        IMapItem CreateMapItem();

        IMonster CreateMonster(int moverId, int mapId, int mapLayerId, Vector3 position, IMapRespawnRegion respawnRegion);

        INpc CreateNpc(IMapNpcObject npcObject, int mapId, int mapLayerId);
    }
}
