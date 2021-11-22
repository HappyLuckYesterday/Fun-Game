using LiteNetwork.Protocol;
using LiteNetwork.Protocol.Abstractions;
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration;
using Rhisis.LoginServer.Core.Abstractions;
using Rhisis.Network.Core;
using Rhisis.Network.Core.Servers;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;
using System;
using System.Linq;

namespace Rhisis.LoginServer.Core.Handlers
{
    [Handler]
    public class AuthenticateHandler
    {
        private readonly ICoreServer _coreServer;
        private readonly IOptions<CoreConfiguration> _coreOptions;

        public AuthenticateHandler(ICoreServer coreServer, IOptions<CoreConfiguration> coreOptions)
        {
            _coreServer = coreServer;
            _coreOptions = coreOptions;
        }

        [HandlerAction(CorePacketType.Authenticate)]
        public void OnExecute(CoreServerClient coreClient, ILitePacketStream packet)
        {
            var corePassword = packet.ReadString();

            if (!_coreOptions.Value.Password.ToLowerInvariant().Equals(corePassword.ToLowerInvariant()))
            {
                SendAuthenticationResult(coreClient, CoreAuthenticationResultType.FailedWrongPassword);
                return;
            }

            var serverType = (ServerType)packet.ReadByte();

            coreClient.ServerInfo = serverType switch
            {
                ServerType.Cluster => ReadClusterInformation(coreClient, packet),
                ServerType.World => ReadWorldInformation(coreClient, packet),
                _ => throw new NotImplementedException()
            };

            if (coreClient.ServerInfo != null)
            {
                SendAuthenticationResult(coreClient, CoreAuthenticationResultType.Success);
            }
        }

        private void SendAuthenticationResult(CoreServerClient client, CoreAuthenticationResultType result)
        {
            using var packet = new LitePacket();

            packet.WriteByte((byte)CorePacketType.AuthenticationResult);
            packet.WriteByte((byte)result);

            client.Send(packet);
        }

        private ServerDescriptor ReadClusterInformation(CoreServerClient client, ILitePacketStream packet)
        {
            var serverId = packet.ReadByte();

            if (_coreServer.Clusters.Any(x => x.Id == serverId))
            {
                SendAuthenticationResult(client, CoreAuthenticationResultType.FailedClusterExists);

                throw new InvalidOperationException($"Cluster with id '{serverId}' already exists.");
            }

            var name = packet.ReadString();
            var host = packet.ReadString();
            var port = packet.ReadUInt16();

            return new Cluster
            {
                Id = serverId,
                Name = name,
                Host = host,
                Port = port
            };
        }

        private ServerDescriptor ReadWorldInformation(CoreServerClient client, ILitePacketStream packet)
        {
            var serverId = packet.ReadByte();
            var clusterServerId = packet.ReadByte();

            Cluster cluster = _coreServer.Clusters.FirstOrDefault(x => x.Id == clusterServerId);

            if (cluster is null)
            {
                SendAuthenticationResult(client, CoreAuthenticationResultType.FailedUnknownServer);
                throw new InvalidOperationException($"Cannot find cluster with id '{clusterServerId}'.");
            }

            if (cluster.Channels.Any(x => x.Id == serverId))
            {
                SendAuthenticationResult(client, CoreAuthenticationResultType.FailedWorldExists);
                throw new InvalidOperationException($"World channel with id '{serverId}' already exists.");
            }

            var name = packet.ReadString();
            var host = packet.ReadString();
            var port = packet.ReadUInt16();

            return new WorldChannel
            {
                Id = serverId,
                ClusterId = clusterServerId,
                Name = name,
                Host = host,
                Port = port
            };
        }
    }
}
