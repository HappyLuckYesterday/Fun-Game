using LiteNetwork.Protocol.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using System;
using System.Collections.Generic;

namespace Rhisis.WorldServer.Abstractions
{
    /// <summary>
    /// Provides an interface to manage the world server.
    /// </summary>
    public interface IWorldServer
    {
        /// <summary>
        /// Gets the connected players collection.
        /// </summary>
        IEnumerable<IPlayer> ConnectedPlayers { get; }

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

        /// <summary>
        /// Disconnects the user identified by the given id.
        /// </summary>
        /// <param name="userId"></param>
        void DisconnectUser(Guid userId);

        /// <summary>
        /// Sends a packet to every connected users.
        /// </summary>
        /// <param name="packet"></param>
        void SendToAll(ILitePacketStream packet);
    }
}
