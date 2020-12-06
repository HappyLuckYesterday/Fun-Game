using Rhisis.Game.Abstractions.Entities;
using Sylver.Network.Common;

namespace Rhisis.WorldServer.Client
{
    public interface IWorldServerClient : INetUser
    {
        /// <summary>
        /// Gets the ID assigned to this session.
        /// </summary>
        uint SessionId { get; }

        /// <summary>
        /// Gets or sets the player entity.
        /// </summary>
        IPlayer Player { get; }
    }
}
