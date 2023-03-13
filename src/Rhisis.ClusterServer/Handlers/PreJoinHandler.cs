using Microsoft.Extensions.Logging;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Infrastructure.Persistance.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using Rhisis.Protocol.Packets.Cluster.Client;
using System.Linq;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.PRE_JOIN)]
internal class PreJoinHandler : ClusterHandlerBase, IPacketHandler
{
    private readonly ILogger<PreJoinHandler> _logger;
    private readonly IAccountDatabase _accountDatabase;
    private readonly IGameDatabase _gameDatabase;
    private readonly ICluster _cluster;

    public PreJoinHandler(ILogger<PreJoinHandler> logger, IAccountDatabase accountDatabase, IGameDatabase gameDatabase, ICluster cluster)
    {
        _logger = logger;
        _accountDatabase = accountDatabase;
        _gameDatabase = gameDatabase;
        _cluster = cluster;
    }

    public void Execute(PreJoinPacket packet)
    {
        AccountEntity account = _accountDatabase.Accounts.SingleOrDefault(x => x.Username == packet.Username);

        if (account is null)
        {
            _logger.LogWarning($"Cannot find account with username '{packet.Username}'.");
            User.Disconnect();
            return;
        }

        PlayerEntity player = _gameDatabase.Players.SingleOrDefault(x => x.AccountId == account.Id && x.Id == packet.CharacterId && x.Name == packet.CharacterName);

        if (player is null)
        {
            _logger.LogWarning($"Unable to prejoin with player '{packet.CharacterName}' for user '{packet.Username}'. Reason: Player not found.");
            User.Disconnect();
            return;
        }

        if (player.IsDeleted)
        {
            _logger.LogWarning($"Unable to prejoin with player '{packet.CharacterName}' for user '{packet.Username}'. Reason: Player is deleted.");
            User.Disconnect();
            return;
        }

        if (_cluster.Configuration.LoginProtectEnabled && !User.IsSecondPasswordCorrect(player.BankCode, packet.BankCode))
        {
            _logger.LogWarning($"Unable to prejoin player '{player.Name}' for user '{packet.Username}'. Reason: bad bank code.");
            User.SendNewNumPad();
            return;
        }

        User.SendPreJoin();
    }
}
