using Rhisis.Game.Abstractions.Entities;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Map
{
    public interface IMapLayer
    {
        /// <summary>
        /// Gets the list of all players of the current layer.
        /// </summary>
        IEnumerable<IPlayer> Players { get; }

        /// <summary>
        /// Gets the list of all monsters of the current layer.
        /// </summary>
        IEnumerable<IMonster> Monsters { get; }

        IMap ParentMap { get; }

        void Process();

        void AddPlayer(IPlayer player);

        void RemovePlayer(IPlayer player);

        void AddMonster(IMonster monster);

        void RemoveMonster(IMonster monster);
    }
}
