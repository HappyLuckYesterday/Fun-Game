using Microsoft.Extensions.Logging;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Game.Protocol.Packets.Cluster.Client;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Infrastructure.Persistance.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System.Linq;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.GETPLAYERLIST)]
internal class GetPlayerListHandler : ClusterHandlerBase
{
    private readonly ILogger<GetPlayerListHandler> _logger;
    private readonly IAccountDatabase _accountDatabase;
    private readonly ICluster _cluster;

    public GetPlayerListHandler(ILogger<GetPlayerListHandler> logger, IAccountDatabase accountDatabase, ICluster cluster)
    {
        _logger = logger;
        _accountDatabase = accountDatabase;
        _cluster = cluster;
    }

    public void Execute(GetPlayerListPacket packet)
    {
        WorldChannel selectedChannel = _cluster.GetChannel(packet.ServerId);

        if (selectedChannel is null)
        {
            _logger.LogWarning($"Unable to get characters list for user '{packet.Username}'. " +
                "Reason: client requested the list on a not connected World server.");
            User.Disconnect();
            return;
        }

        AccountEntity userAccount = _accountDatabase.Accounts.SingleOrDefault(x => x.Username == packet.Username && x.Password == packet.Password);

        if (userAccount is null)
        {
            _logger.LogWarning($"[SECURITY] Unable to load character list for user '{packet.Username}'. " +
                "Reason: bad presented credentials compared to the database.");
            User.Disconnect();
            return;
        }

        User.AccountId = userAccount.Id;

        User.SendPlayerList();
        User.SendChannelIpAddress(selectedChannel.Ip);

        if (_cluster.Configuration.LoginProtectEnabled)
        {
            User.SendLoginProtect();
        }
    }
}
