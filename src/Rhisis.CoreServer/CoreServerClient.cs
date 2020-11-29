using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Network.Core;
using Rhisis.Network.Core.Servers;
using Sylver.Network.Data;
using Sylver.Network.Server;
using System;
using System.Linq;
using System.Net.Sockets;

namespace Rhisis.CoreServer
{
    public class CoreServerClient : NetServerClient
    {
        private IServiceProvider _serviceProvider;
        private ILogger<CoreServerClient> _logger;
        private ICoreServer _coreServer;

        public ServerType ServerType => ServerInfo?.ServerType ?? ServerType.Unknown;

        public ServerDescriptor ServerInfo { get; private set; }

        public CoreServerClient(Socket socketConnection) 
            : base(socketConnection)
        {
        }

        public override void HandleMessage(INetPacketStream packet)
        {
            try
            {
                var packetHeader = (CorePacketType)packet.ReadByte();

                if (packetHeader == CorePacketType.Authenticate)
                {
                    string corePassword = packet.ReadString();

                    if (!_coreServer.Configuration.Password.ToLowerInvariant().Equals(corePassword.ToLowerInvariant()))
                    {
                        SendAuthenticationResult(CoreAuthenticationResultType.FailedWrongPassword);
                        return;
                    }

                    var serverType = (ServerType)packet.ReadByte();

                    ServerInfo = serverType switch
                    {
                        ServerType.Cluster => ReadClusterInformation(packet),
                        ServerType.World => ReadWorldInformation(packet),
                        _ => throw new NotImplementedException()
                    };

                    if (ServerInfo != null)
                    {
                        SendAuthenticationResult(CoreAuthenticationResultType.Success);
                        _coreServer.SendServerListUpdate();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while processing a core request.");
            }
        }

        public void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetService<ILogger<CoreServerClient>>();
            _coreServer = _serviceProvider.GetService<ICoreServer>();

            using var packet = new NetPacket();
            packet.WriteByte((byte)CorePacketType.Welcome);
            Send(packet);
        }

        private void SendAuthenticationResult(CoreAuthenticationResultType result)
        {
            using var packet = new NetPacket();

            packet.WriteByte((byte)CorePacketType.AuthenticationResult);
            packet.WriteByte((byte)result);

            Send(packet);
        }

        private ServerDescriptor ReadClusterInformation(INetPacketStream packet)
        {
            byte serverId = packet.ReadByte();

            if (_coreServer.Clusters.Any(x => x.Id == serverId))
            {
                SendAuthenticationResult(CoreAuthenticationResultType.FailedClusterExists);

                throw new InvalidOperationException($"Cluster with id '{serverId}' already exists.");
            }

            string name = packet.ReadString();
            string host = packet.ReadString();
            ushort port = packet.ReadUInt16();

            return new Cluster
            {
                Id = serverId,
                Name = name,
                Host = host,
                Port = port
            };
        }

        private ServerDescriptor ReadWorldInformation(INetPacketStream packet)
        {
            byte serverId = packet.ReadByte();
            byte clusterServerId = packet.ReadByte();

            Cluster cluster = _coreServer.Clusters.FirstOrDefault(x => x.Id == clusterServerId);

            if (cluster is null)
            {
                SendAuthenticationResult(CoreAuthenticationResultType.FailedUnknownServer);
                throw new InvalidOperationException($"Cannot find cluster with id '{clusterServerId}'.");
            }

            if (cluster.Channels.Any(x => x.Id == serverId))
            {
                SendAuthenticationResult(CoreAuthenticationResultType.FailedWorldExists);
                throw new InvalidOperationException($"World channel with id '{serverId}' already exists.");
            }

            string name = packet.ReadString();
            string host = packet.ReadString();
            ushort port = packet.ReadUInt16();

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
