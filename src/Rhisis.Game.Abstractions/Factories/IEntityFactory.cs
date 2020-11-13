using Rhisis.Core.Structures;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Map;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;

namespace Rhisis.Game.Abstractions.Factories
{
    /// <summary>
    /// Provides a factory mechanism to create entities.
    /// </summary>
    public interface IEntityFactory
    {
        /// <summary>
        /// Creates a new map item on the given map layer.
        /// </summary>
        /// <param name="item">Item to create.</param>
        /// <param name="mapLayer">Map layer where the item will be living.</param>
        /// <param name="owner">Item owner.</param>
        /// <param name="position">Item position.</param>
        /// <returns>The created map item entity.</returns>
        IMapItem CreateMapItem(IItem item, IMapLayer mapLayer, IWorldObject owner, Vector3 position);

        /// <summary>
        /// Creates a new temporary map item on the given map layer.
        /// </summary>
        /// <param name="item">Item to create.</param>
        /// <param name="mapLayer">Map layer where the item will be living.</param>
        /// <param name="owner">Item owner.</param>
        /// <param name="position">Item position.</param>
        /// <param name="despawnTime">Despawn time.</param>
        /// <returns>The created map item entity.</returns>
        IMapItem CreateTemporaryMapItem(IItem item, IMapLayer mapLayer, IWorldObject owner, Vector3 position, int despawnTime);

        /// <summary>
        /// Creates a permanent map item on the given layer.
        /// </summary>
        /// <param name="item">Item to create.</param>
        /// <param name="mapLayer">Map layer where the item will be living.</param>
        /// <param name="owner">Item owner.</param>
        /// <param name="region">Item respawn region.</param>
        /// <returns>The created map item.</returns>
        IMapItem CreatePermanentMapItem(IItem item, IMapLayer mapLayer, IWorldObject owner, IMapRespawnRegion region);

        /// <summary>
        /// Creates a new monster entity on the given map layer.
        /// </summary>
        /// <param name="moverId">Monster mover id.</param>
        /// <param name="mapId">Map id.</param>
        /// <param name="mapLayerId">Map layer id.</param>
        /// <param name="position">Monster position.</param>
        /// <param name="respawnRegion">Monster respawn region.</param>
        /// <returns>The created monster.</returns>
        IMonster CreateMonster(int moverId, int mapId, int mapLayerId, Vector3 position, IMapRespawnRegion respawnRegion);

        /// <summary>
        /// Creates a NPC on the given map layer.
        /// </summary>
        /// <param name="npcObject">Npc object to create.</param>
        /// <param name="mapId">Map id.</param>
        /// <param name="mapLayerId">Map layer.</param>
        /// <returns>The created NPC.</returns>
        INpc CreateNpc(IMapNpcObject npcObject, int mapId, int mapLayerId);

        /// <summary>
        /// Creates a new item with the given attributes.
        /// </summary>
        /// <param name="itemId">Item id.</param>
        /// <param name="refine">Item refine bonus.</param>
        /// <param name="element">Item element.</param>
        /// <param name="elementRefine">Item element refine bonus.</param>
        /// <param name="creatorId">Item creator id.</param>
        /// <param name="quantity">Item quantity.</param>
        /// <returns>The created item.</returns>
        IItem CreateItem(int itemId, byte refine, ElementType element, byte elementRefine, int creatorId = -1, int quantity = 1);

        /// <summary>
        /// Creates a new gold item with a given amount.
        /// </summary>
        /// <param name="amount">Gold amount.</param>
        /// <returns>The created gold item.</returns>
        IItem CreateGoldItem(int amount);
    }
}
