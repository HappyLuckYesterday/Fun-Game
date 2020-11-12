using Sylver.Network.Common;
using Sylver.Network.Data;

namespace Rhisis.Game.Abstractions.Protocol
{
    /// <summary>
    /// Provides an abstraction for a game connection.
    /// </summary>
    public interface IGameConnection : INetUser
    {
        /// <summary>
        /// Gets the ID assigned to this session.
        /// </summary>
        uint SessionId { get; }
    }
}
