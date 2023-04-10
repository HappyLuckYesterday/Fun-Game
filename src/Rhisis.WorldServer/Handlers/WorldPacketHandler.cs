using Rhisis.Game.Entities;

namespace Rhisis.WorldServer.Handlers;

internal class WorldPacketHandler
{
    public WorldUser User { get; set; }

    public Player Player => User.Player;

    protected WorldPacketHandler()
    {
    }
}
