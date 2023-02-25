using LiteNetwork.Server;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Extensions;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rhisis.LoginServer.Caching;

public sealed class ClusterCacheUser : LiteServerUser
{
    private readonly ILogger<ClusterCacheUser> _logger;
    private readonly ClusterCacheServer _cacheServer;

    public ClusterInfo Cluster { get; }

    public ClusterCacheUser(ILogger<ClusterCacheUser> logger, ClusterCacheServer cacheServer)
    {
        _logger = logger;
        _cacheServer = cacheServer;
    }

    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        using BinaryReader reader = new(new MemoryStream(packetBuffer));

        CorePacketType packetType = (CorePacketType)reader.ReadByte();
        
        try
        {
            string messageContent = reader.ReadString();
            object message = JsonSerializer.Deserialize<object>(messageContent);

            //Type packetHandlerType = PacketHandlerCache.GetCorePacketType(packetType);

            //if (packetHandlerType is null)
            //{
            //    throw new InvalidOperationException("Failed to find packet handler type.");
            //}

            //MessageHandler packetHandler = _messageDispatcher.GetHandler(packetHandlerType);

            //if (packetHandler is null)
            //{
            //    throw new InvalidOperationException("Failed to find packet handler.");
            //}

            //packetHandler.Target.AssignProperty("User", this);
            //packetHandler.Target.AssignProperty("Server", _cacheServer);

            //packetHandler.Execute(message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"An error occured while parsing packet '{packetType}'.");
        }
        
        return base.HandleMessageAsync(packetBuffer);
    }

    public void Send(CorePacketType packet, object message)
    {
        using BinaryWriter writer = new(new MemoryStream());
        writer.Write((byte)packet);
        writer.Write(JsonSerializer.Serialize(message));

        Send(writer.BaseStream);
    }
}