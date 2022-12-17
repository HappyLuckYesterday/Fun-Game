using LiteNetwork;
using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Abstractions.Server;
using Rhisis.LoginServer.Abstractions;
using Rhisis.Protocol.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.LoginServer;

public class CoreServer : LiteServer<CoreUser>, ICoreServer
{
    private readonly ILogger<CoreServer> _logger;

    public IEnumerable<Cluster> Clusters => Users.Cast<CoreUser>().Select(x => x.Cluster);

    public CoreServer(LiteServerOptions serverOptions, ILogger<CoreServer> logger, IServiceProvider serviceProvider)
        : base(serverOptions, serviceProvider)
    {
        _logger = logger;
    }

    protected override void OnAfterStart()
    {
        _logger.LogInformation($"CoreServer started and listening on port '{Options.Port}'.");
    }

    protected override void OnError(LiteConnection connection, Exception exception)
    {
        _logger.LogError(exception, $"An exception occured in {typeof(CoreServer).Name}.");
    }

    public void SendToClusters(CorePacket packet)
    {
        SendTo(Users, packet.GetBuffer());
    }
}
