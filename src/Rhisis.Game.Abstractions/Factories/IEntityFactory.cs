using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;

namespace Rhisis.Game.Abstractions.Factories
{
    public interface IEntityFactory
    {
        IMapItem CreateMapItem(IItem item, IMapLayer mapLayer, IWorldObject owner, Vector3 position);

        IMapItem CreateTemporaryMapItem(IItem item, IMapLayer mapLayer, IWorldObject owner, Vector3 position, int despawnTime);

        IMapItem CreatePermanentMapItem(IItem item, IMapLayer mapLayer, IWorldObject owner, IMapRespawnRegion region);

        IMonster CreateMonster(int moverId, int mapId, int mapLayerId, Vector3 position, IMapRespawnRegion respawnRegion);

        INpc CreateNpc(IMapNpcObject npcObject, int mapId, int mapLayerId);

        IItem CreateItem(int itemId, byte refine, ElementType element, byte elementRefine, int creatorId = -1, int quantity = 1);

        IItem CreateGoldItem(int amount);
    }
}
