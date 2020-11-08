using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions.Systems
{
    public interface IInventoryItemUsage
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
