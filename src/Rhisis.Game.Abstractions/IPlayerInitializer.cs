using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions
{
    /// <summary>
    /// Provides a mechanism to load and save player's data.
    /// </summary>
    public interface IPlayerInitializer
    {
        /// <summary>
        /// Load player data.
        /// </summary>
        /// <param name="player"></param>
        void Load(IPlayer player);

        /// <summary>
        /// Save player save.
        /// </summary>
        /// <param name="player"></param>
        void Save(IPlayer player);
    }
}
