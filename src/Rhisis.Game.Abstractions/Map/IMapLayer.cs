using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Entities;
using System;
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

        /// <summary>
        /// Adds an item to the current map layer.
        /// </summary>
        /// <param name="mapItem">Map item to add.</param>
        /// <exception cref="ArgumentNullException"></exception>
        void AddItem(IMapItem mapItem);

        /// <summary>
        /// Adds a monster to the current map layer.
        /// </summary>
        /// <param name="monster">Monster entity to add.</param>
        /// <exception cref="ArgumentNullException"></exception>
        void AddMonster(IMonster monster);

        /// <summary>
        /// Adds a NPC to the current map layer.
        /// </summary>
        /// <param name="npc">Npc to add.</param>
        /// <exception cref="ArgumentNullException"></exception>
        void AddNpc(INpc npc);

        /// <summary>
        /// Adds a player to the current map layer.
        /// </summary>
        /// <param name="player">Player to add.</param>
        /// <exception cref="ArgumentNullException"></exception>
        void AddPlayer(IPlayer player);

        /// <summary>
        /// Removes a map item from the current map layer.
        /// </summary>
        /// <param name="mapItem">Map item to remove.</param>
        /// <exception cref="ArgumentNullException"></exception>
        void RemoveItem(IMapItem mapItem);

        /// <summary>
        /// Removes an existing monster from the current map layer.
        /// </summary>
        /// <param name="monster">Monster to remove.</param>
        /// <exception cref="ArgumentNullException"></exception>
        void RemoveMonster(IMonster monster);

        /// <summary>
        /// Removes an existing NPC from the current map layer.
        /// </summary>
        /// <param name="npc">NPC to remove.</param>
        /// <exception cref="ArgumentNullException"></exception>
        void RemoveNpc(INpc npc);

        /// <summary>
        /// Removes an existing player from the current map layer.
        /// </summary>
        /// <param name="player">Player to remove.</param>
        /// <exception cref="ArgumentNullException"></exception>
        void RemovePlayer(IPlayer player);

        /// <summary>
        /// Process the map layer logic.
        /// </summary>
        void Process();

        /// <summary>
        /// Gets a collection of visible objects for the given world object.
        /// </summary>
        /// <param name="worldObject">World object.</param>
        /// <returns>Collection of visible objects.</returns>
        IEnumerable<IWorldObject> GetVisibleObjects(IWorldObject worldObject);
    }
}
