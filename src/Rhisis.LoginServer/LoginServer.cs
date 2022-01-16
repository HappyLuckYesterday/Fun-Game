using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Infrastructure.Persistance;
using Rhisis.LoginServer.Abstractions;
using System;
using System.Linq;

namespace Rhisis.LoginServer
{
    public sealed class LoginServer : LiteServer<LoginUser>, ILoginServer
    {
        private readonly ILogger<LoginServer> _logger;
        private readonly IRhisisDatabase _database;

        /// <summary>
        /// Creates a new <see cref="LoginServer"/> instance.
        /// </summary>
        /// <param name="serverOptions">Server options.</param>
        /// <param name="logger">Logger</param>
        /// <param name="database"></param>
        public LoginServer(LiteServerOptions serverOptions, ILogger<LoginServer> logger, IRhisisDatabase database)
            : base(serverOptions)
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

        public ILoginUser GetClientByUsername(string username)
            => ConnectedUsers.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        public bool IsClientConnected(string username) => GetClientByUsername(username) is not null;
    }
}