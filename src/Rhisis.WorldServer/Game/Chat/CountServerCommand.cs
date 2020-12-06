using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Common;
using Rhisis.Network.Snapshots;

namespace Rhisis.WorldServer.Game.Chat
{
    [ChatCommand("/count", AuthorityType.GameMaster)]
    [ChatCommand("/cnt", AuthorityType.GameMaster)]
    public class CountCommand : IChatCommand
    {
        private readonly IWorldServer _worldServer;

        /// <summary>
        /// Creates a new <see cref="CountCommand"/> instance.
        /// </summary>
        /// <param name="worldServer">World server.</param>
        public CountCommand(IWorldServer worldServer)
        {
            _worldServer = worldServer;
        }

        /// <inheritdoc />
        public void Execute(IPlayer player, object[] parameters)
        {
            var onlineCount = _worldServer.GetOnlineConnectedPlayerNumber();

            using var worldMessageSnapshot = new WorldMessageSnapshot(player, $"Players Online: {onlineCount}");
            player.Send(worldMessageSnapshot);
        }
    }
}