using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Extensions;
using Rhisis.LoginServer.Core.Handlers;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Core;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rhisis.LoginServer.Core;

public sealed class CoreCacheUser : LiteServerUser
{
    private readonly ILogger<CoreCacheUser> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ClusterInfo Cluster { get; set; }

    public CoreCacheUser(ILogger<CoreCacheUser> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        using BinaryReader reader = new(new MemoryStream(packetBuffer));

        CorePacketType packetType = (CorePacketType)reader.ReadByte();

        try
        {
            string messageContent = reader.ReadString();

            switch (packetType)
            {
                case CorePacketType.UpdateClusterInfo:
                    ClusterInfoUpdateHandler clusterInfoUpdateHandler = _serviceProvider.CreateInstance<ClusterInfoUpdateHandler>();
                    clusterInfoUpdateHandler.User = this;
                    clusterInfoUpdateHandler.Execute(JsonSerializer.Deserialize<ClusterInfoUpdatePacket>(messageContent));
                    break;
                default:
                    _logger.LogWarning($"No handler for packet: '{packetType}'.");
                    break;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"An error occured while parsing packet '{packetType}'.");
        }

        return base.HandleMessageAsync(packetBuffer);
    }

    public void Send(CorePacketType packet, object message = null)
    {
        using BinaryWriter writer = new(new MemoryStream());
        writer.Write((byte)packet);

        if (message is not null)
        {
            writer.Write(JsonSerializer.Serialize(message));
        }

        Send(writer.BaseStream);
    }
}