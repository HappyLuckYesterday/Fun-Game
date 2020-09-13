using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;

namespace Rhisis.World.Systems.Drop
{
    public interface IDropSystem
    {
        /// <summary>
        /// Drops an item on the ground.
        /// </summary>
        /// <param name="entity">Current entity.</param>
        /// <param name="itemId">Item id to drop.</param>
        /// <param name="owner">Item owner.</param>
        /// <param name="quantity">Item quantity.</param>
        void DropItem(IWorldEntity entity, int itemId, IWorldEntity owner, int quantity = 1);

        /// <summary>
        /// Drops an item to the ground.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="item">Item to drop.</param>
        /// <param name="owner">Owner.</param>
        /// <param name="quantity">Quantity of items to drop.</param>
        void DropItem(IWorldEntity entity, Item item, IWorldEntity owner, int quantity = 1);

        /// <summary>
        /// Drops gold on the group.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="goldAmount">Gold amount to drop.</param>
        /// <param name="owner">Owner.</param>
        void DropGold(IWorldEntity entity, int goldAmount, IWorldEntity owner);
    }
}
