using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Common;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.LoginServer.Client;
using Rhisis.LoginServer.Packets;
using Rhisis.Network;
using Rhisis.Network.Core.Servers;
using Rhisis.Network.Packets.Login;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.LoginServer.Handlers
{
    [Handler]
    public class CertifyHandler
    {
        private readonly ILogger<CertifyHandler> _logger;
        private readonly LoginConfiguration _loginConfiguration;
        private readonly ILoginServer _loginServer;
        private readonly IRhisisDatabase _database;
        private readonly ILoginPacketFactory _loginPacketFactory;

        /// <summary>
        /// Creates a new <see cref="CertifyHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="loginConfiguration">Login server configuration.</param>
        /// <param name="loginServer">Login server instance.</param>
        /// <param name="database">Database service.</param>
        /// <param name="coreServer">Core server.</param>
        /// <param name="loginPacketFactory">Login server packet factory.</param>
        public CertifyHandler(ILogger<CertifyHandler> logger, IOptions<LoginConfiguration> loginConfiguration, ILoginServer loginServer, IRhisisDatabase database, ILoginPacketFactory loginPacketFactory)
        {
            _logger = logger;
            _loginConfiguration = loginConfiguration.Value;
            _loginServer = loginServer;
            _database = database;
            _loginPacketFactory = loginPacketFactory;
        }

        /// <summary>
        /// Certifies and authenticates a user.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="certifyPacket">Certify packet.</param>
        [HandlerAction(PacketType.CERTIFY)]
        public void Execute(ILoginClient client, CertifyPacket certifyPacket)
        {
            if (certifyPacket.BuildVersion != _loginConfiguration.BuildVersion)
            {
                AuthenticationFailed(client, ErrorType.ILLEGAL_VER, "bad client build version");
                return;
            }

            DbUser user = _database.Users.FirstOrDefault(x => x.Username.Equals(certifyPacket.Username));
            AuthenticationResult authenticationResult = Authenticate(user, certifyPacket.Password);

            switch (authenticationResult)
            {
                case AuthenticationResult.BadUsername:
                    AuthenticationFailed(client, ErrorType.FLYFF_ACCOUNT, "bad username");
                    break;
                case AuthenticationResult.BadPassword:
                    AuthenticationFailed(client, ErrorType.FLYFF_PASSWORD, "bad password");
                    break;
                case AuthenticationResult.AccountSuspended:
                    // TODO
                    break;
                case AuthenticationResult.AccountTemporarySuspended:
                    // TODO
                    break;
                case AuthenticationResult.AccountDeleted:
                    AuthenticationFailed(client, ErrorType.ILLEGAL_ACCESS, "logged in with deleted account");
                    break;
                case AuthenticationResult.Success:
                    if (_loginServer.IsClientConnected(certifyPacket.Username))
                    {
                        AuthenticationFailed(client, ErrorType.DUPLICATE_ACCOUNT, "client already connected");
                        return;
                    }

                    user.LastConnectionTime = DateTime.UtcNow;
                    _database.Users.Update(user);
                    _database.SaveChanges();

                    IEnumerable<Cluster> clusters = _loginServer.ConnectedClusters.Values.OrderBy(x => x.Id);

                    _loginPacketFactory.SendServerList(client, certifyPacket.Username, clusters);
                    client.SetClientUsername(certifyPacket.Username);
                    _logger.LogInformation($"User '{client.Username}' logged succesfully from {client.Socket.RemoteEndPoint}.");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sends an authentication failed packet to the client.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="error">Authentication error type.</param>
        /// <param name="reason">Authentication error reason.</param>
        private void AuthenticationFailed(ILoginClient client, ErrorType error, string reason)
        {
            _logger.LogWarning($"Unable to authenticate user from {client.Socket.RemoteEndPoint}. Reason: {reason}");
            _loginPacketFactory.SendLoginError(client, error);
        }

        /// <summary>
        /// Authenticates a user.
        /// </summary>
        /// <param name="user">Database user.</param>
        /// <param name="password">Password</param>
        /// <returns>Authentication result</returns>
        private AuthenticationResult Authenticate(DbUser user, string password)
        {
            if (user == null)
            {
                return AuthenticationResult.BadUsername;
            }

            if (user.IsDeleted)
            {
                return AuthenticationResult.AccountDeleted;
            }

            if (!user.Password.Equals(password, StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticationResult.BadPassword;
            }

            return AuthenticationResult.Success;
        }
    }
}