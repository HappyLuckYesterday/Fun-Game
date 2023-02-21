using LiteNetwork.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Infrastructure.Persistance;
using System;
using System.Linq;

namespace Rhisis.LoginServer;

public sealed class LoginServer : LiteServer<LoginUser>
{
    private readonly ILogger<LoginServer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public LoginServer(LiteServerOptions options, ILogger<LoginServer> logger, IServiceProvider serviceProvider = null)
        : base(options, serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override void OnBeforeStart()
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        IAccountDatabase accountDatabase = scope.ServiceProvider.GetService<IAccountDatabase>();

        accountDatabase.Migrate();

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

