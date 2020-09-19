using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Entities;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions.Map
{
    public interface IMapLayer
    {
        /// <summary>
        /// Gets the map layer id.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the parent map.
        /// </summary>
        IMap ParentMap { get; }

        /// <summary>
        /// Gets the list of all items of the current layer.
        /// </summary>
        IEnumerable<IMapItem> Items { get; }

        /// <summary>
        /// Gets the list of all monsters of the current layer.
        /// </summary>
        IEnumerable<IMonster> Monsters { get; }

        /// <summary>
        /// Gets the list of all NPCs of the current layer.
        /// </summary>
        IEnumerable<INpc> Npcs { get; }

        /// <summary>
        /// Gets the list of all players of the current layer.
        /// </summary>
        IEnumerable<IPlayer> Players { get; }

        void AddItem(IMapItem mapItem);

        void AddMonster(IMonster monster);

        void AddNpc(INpc npc);

        void AddPlayer(IPlayer player);

        void RemoveItem(IMapItem mapItem);

        void RemoveMonster(IMonster monster);

        void RemoveNpc(INpc npc);

        void RemovePlayer(IPlayer player);

        void Process();

        /// <summary>
        /// Gets a collection of visible objects for the given world object.
        /// </summary>
        /// <param name="worldObject">World object.</param>
        /// <returns>Collection of visible objects.</returns>
        IEnumerable<IWorldObject> GetVisibleObjects(IWorldObject worldObject);
    }
}
