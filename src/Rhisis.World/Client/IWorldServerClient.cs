using Rhisis.Game.Abstractions.Entities;
using Rhisis.World.Game.Entities;
using Sylver.Network.Common;
using System;

namespace Rhisis.World.Client
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
        [Obsolete]
        IPlayerEntity Player { get; set; }

        IPlayer NewPlayer { get; }
    }
}
