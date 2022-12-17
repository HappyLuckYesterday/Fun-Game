using Rhisis.Abstractions.Entities;
using Rhisis.Abstractions.Protocol;

namespace Rhisis.WorldServer.Abstractions;

public interface IWorldServerUser
{
    /// <summary>
    /// Gets the ID assigned to this session.
    /// </summary>
    uint SessionId { get; }

    /// <summary>
    /// Gets or sets the player entity.
    /// </summary>
    IPlayer Player { get; }

    /// <summary>
    /// Sends the packet to the current user.
    /// </summary>
    /// <param name="packet"></param>
    void Send(IFFPacket packet);
}
