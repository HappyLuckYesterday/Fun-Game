using Rhisis.Core.Data;
using Rhisis.Core.Structures.Game;
using Rhisis.Database.Entities;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Maps;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Game.Factories
{
    public interface IItemFactory
    {
        /// <summary>
        /// Creates a new Item entity on the given context.
        /// </summary>
        /// <param name="currentMapContext">Map context.</param>
        /// <param name="currentMapLayerContext">Map layer context.</param>
        /// <param name="item">Item model.</param>
        /// <param name="owner">Item owner.</param>
        /// <returns>New item entity.</returns>
        IItemEntity CreateItemEntity(IMapInstance currentMapContext, IMapLayer currentMapLayerContext, Item item, IWorldEntity owner = null);

        /// <summary>
        /// Cretes a new <see cref="Item"/>.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="creatorId">Creator id.</param>
        /// <returns>New Item.</returns>
        Item CreateItem(int id, int creatorId = -1);

        /// <summary>
        /// Creates a new <see cref="Item"/>.
        /// </summary>
        /// <param name="id">Item id.</param>
        /// <param name="refine">Item refine.</param>
        /// <param name="element">Item element.</param>
        /// <param name="elementRefine">Item element refine.</param>
        /// <param name="creatorId">Creator id.</param>
        /// <returns>New item.</returns>
        Item CreateItem(int id, byte refine, ElementType element, byte elementRefine, int creatorId = -1);

        /// <summary>
        /// Creates a new <see cref="Item"/> using it's Item name.
        /// </summary>
        /// <param name="name">Item name.</param>
        /// <param name="refine">Item refine.</param>
        /// <param name="element">Item element.</param>
        /// <param name="elementRefine">Item element refine.</param>
        /// <param name="creatorId">Creator id.</param>
        /// <returns>New item.</returns>
        Item CreateItem(string name, byte refine, ElementType element, byte elementRefine, int creatorId = -1);

        /// <summary>
        /// Creates a new <see cref="Item"/> from a <see cref="DbItemStorage"/> instance.
        /// </summary>
        /// <param name="databaseItem">Database item.</param>
        /// <returns>New item.</returns>
        Item CreateItem(DbItemStorage databaseItem);

        /// <summary>
        /// Creates a new <see cref="InventoryItem"/> form a <see cref="DbItemStorage"/> instance.
        /// </summary>
        /// <param name="databaseItem">Database item.</param>
        /// <returns>New inventory item.</returns>
        InventoryItem CreateInventoryItem(DbItemStorage databaseItem);
    }
}
