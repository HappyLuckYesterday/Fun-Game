using LiteNetwork;
using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Infrastructure.Persistance;
using Rhisis.LoginServer.Abstractions;
using System;
using System.Linq;

namespace Rhisis.LoginServer;

public sealed class LoginServer : LiteServer<LoginUser>, ILoginServer
{
    private readonly ILogger<LoginServer> _logger;
    private readonly IRhisisDatabase _database;

    /// <summary>
    /// Creates a new <see cref="LoginServer"/> instance.
    /// </summary>
    /// <param name="serverOptions">Server options.</param>
    /// <param name="serviceProvider">Service provider.</param>
    /// <param name="logger">Logger</param>
    /// <param name="database"></param>
    public LoginServer(LiteServerOptions serverOptions, IServiceProvider serviceProvider, ILogger<LoginServer> logger, IRhisisDatabase database)
        : base(serverOptions, serviceProvider)
    {
        _logger = logger;
        _database = database;
    }

    protected override void OnBeforeStart()
    {
        if (!_database.IsAlive())
        {
            throw new InvalidProgramException($"Cannot start {nameof(LoginServer)}. Failed to reach database.");
        }
    }

    protected override void OnAfterStart()
    {
        _logger.LogInformation($"{nameof(LoginServer)} is started and listen on {Options.Host}:{Options.Port}.");
    }

    protected override void OnError(LiteConnection connection, Exception exception)
    {
        _logger.LogError(exception, $"An exception occured in {typeof(LoginServer).Name}.");
    }

    public ILoginUser GetClientByUsername(string username)
        => Users.Cast<LoginUser>().FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

    public bool IsClientConnected(string username) => GetClientByUsername(username) is not null;
}