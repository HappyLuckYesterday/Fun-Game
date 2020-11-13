using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions.Systems
{
    /// <summary>
    /// Provides a mechanism to manage the player inventory item usage.
    /// </summary>
    public interface IInventoryItemUsageSystem
    {
        /// <summary>
        /// Uses blinkwing items.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="blinkwing"></param>
        void UseBlinkwingItem(IPlayer player, IItem blinkwing);

        /// <summary>
        /// Use food item.
        /// </summary>
        /// <param name="player">Player using the item.</param>
        /// <param name="foodItemToUse">Food item to use.</param>
        void UseFoodItem(IPlayer player, IItem foodItemToUse);

        /// <summary>
        /// Uses fireworks items.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="magicItem"></param>
        void UseMagicItem(IPlayer player, IItem magicItem);

        /// <summary>
        /// Uses perin items.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="perinItem"></param>
        void UsePerin(IPlayer player, IItem perinItem);
    }
}
