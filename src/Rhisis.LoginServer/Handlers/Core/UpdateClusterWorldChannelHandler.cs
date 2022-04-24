using Microsoft.Extensions.Logging;
using Rhisis.Abstractions.Server;
using Rhisis.Protocol.Core;
using Sylver.HandlerInvoker.Attributes;
using System.Linq;

namespace Rhisis.LoginServer.Core.Handlers
{
    [Handler]
    public class UpdateClusterWorldChannelHandler
    {
        private readonly ILogger<UpdateClusterWorldChannelHandler> _logger;

        public UpdateClusterWorldChannelHandler(ILogger<UpdateClusterWorldChannelHandler> logger)
        {
            _logger = logger;
        }

        [HandlerAction(CorePacketType.UpdateClusterWorldChannel)]
        public void OnExecute(CoreUser user, CorePacket packet)
        {
            int channelId = packet.ReadByte();
            string channelName = packet.ReadString();
            string channelHost = packet.ReadString();
            int channelPort = packet.ReadUInt16();
            int channelUsersCount = packet.ReadInt32();
            int maximumUsers = packet.ReadInt32();

            WorldChannel channel = user.Cluster.Channels.SingleOrDefault(x => x.Id == channelId);

            if (channel is null)
            {
                channel = new()
                {
                    Id = channelId,
                    Name = channelName,
                    Host = channelHost,
                    Port = channelPort,
                    ConnectedUsers = channelUsersCount,
                    MaximumUsers = maximumUsers
                };

                user.Cluster.Channels.Add(channel);
                _logger.LogTrace($"Channel '{channel.Name}' added to '{user.Cluster.Name}' cluster.");
            }
            else
            {
                // We do not update other components, since they are immutable.
                channel.ConnectedUsers = channelUsersCount;
                _logger.LogTrace($"Updating channel '{channel.Name}' on cluster '{user.Cluster.Name}'.");
            }
        }
    }
}
