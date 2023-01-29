using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Infrastructure.Persistance;
using System;
using System.Linq;

namespace Rhisis.LoginServer;

internal sealed class LoginServer : LiteServer<LoginUser>
{
    private readonly ILogger<LoginServer> _logger;
    private readonly IAccountDatabase _accountDatabase;

    public LoginServer(LiteServerOptions options, ILogger<LoginServer> logger, IAccountDatabase accountDatabase, IServiceProvider serviceProvider = null)
        : base(options, serviceProvider)
    {
        _logger = logger;
        _accountDatabase = accountDatabase;
    }

    protected override void OnBeforeStart()
    {
        _accountDatabase.Migrate();

        base.OnBeforeStart();
    }

    protected override void OnAfterStart()
    {
        _logger.LogInformation($"Login Server listening on port {Options.Port}.");
    }

    protected override void OnError(LoginUser connection, Exception exception)
    {
        _logger.LogError(exception, $"An exception occured in {typeof(LoginServer).Name}.");
    }

    public bool IsUserConnected(string username) => Users.Cast<LoginUser>().Any(x => x.Username?.Equals(username) ?? false);
}
