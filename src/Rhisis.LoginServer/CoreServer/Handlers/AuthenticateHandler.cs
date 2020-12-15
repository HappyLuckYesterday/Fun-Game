using Rhisis.Network.Core;
using Rhisis.Network.Core.Servers;
using Sylver.HandlerInvoker.Attributes;
using Sylver.Network.Data;
using System;
using System.Linq;

namespace Rhisis.LoginServer.CoreServer.Handlers
{
    [Handler]
    public class AuthenticateHandler
    {
        private readonly ICoreServer _coreServer;

        public AuthenticateHandler(ICoreServer coreServer)
        {
            _coreServer = coreServer;
        }

        [HandlerAction(CorePacketType.Authenticate)]
        public void OnExecute(CoreServerClient coreClient, INetPacketStream packet)
        {
            var corePassword = packet.ReadString();

            if (!_coreServer.Configuration.Password.ToLowerInvariant().Equals(corePassword.ToLowerInvariant()))
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
            using var packet = new NetPacket();

            packet.WriteByte((byte)CorePacketType.AuthenticationResult);
            packet.WriteByte((byte)result);

            client.Send(packet);
        }

        private ServerDescriptor ReadClusterInformation(CoreServerClient client, INetPacketStream packet)
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

        private ServerDescriptor ReadWorldInformation(CoreServerClient client, INetPacketStream packet)
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
