using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Game.Abstractions.Entities;
using Sylver.Network.Server;

namespace Rhisis.WorldServer
{
    /// <summary>
    /// Provides an interface to manage the world server.
    /// </summary>
    public interface IWorldServer : INetServer
    {
        /// <summary>
        /// Gets the core server configuration.
        /// </summary>
        CoreConfiguration CoreConfiguration { get; }

        /// <summary>
        /// Gets the world server configuration.
        /// </summary>
        WorldConfiguration WorldConfiguration { get; }

        /// <summary>
        /// Gets a player entity by his id.
        /// </summary>
        /// <param name="id">Player id</param>
        /// <returns><see cref="IPlayer"/></returns>
        IPlayer GetPlayerEntity(uint id);

        /// <summary>
        /// Gets a player entity by his name
        /// </summary>
        /// <param name="name">Player name.</param>
        /// <returns><see cref="IPlayer"/></returns>
        IPlayer GetPlayerEntity(string name);

        /// <summary>
        /// Gets a player entity by its character id.
        /// </summary>
        /// <param name="id">Character id.</param>
        /// <returns><see cref="IPlayer"/></returns>
        IPlayer GetPlayerEntityByCharacterId(uint id);

        /// <summary>
        /// Gets the number of players connected to the worldserver.
        /// </summary>
        /// <returns><see cref="uint"/></returns>
        uint GetOnlineConnectedPlayerNumber();
    }
}
