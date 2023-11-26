using LiteNetwork.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Extensions;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rhisis.Protocol.Networking;

public class FFInterClient : LiteClient
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;

    public FFInterClient(LiteClientOptions options, ILogger logger, IServiceProvider serviceProvider = null) 
        : base(options, serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        using BinaryReader reader = new(new MemoryStream(packetBuffer));
        string packetHeader = reader.ReadString();

        try
        {
            if (!FFInterServerConnectionHandlers.TryGetHandler(packetHeader, out Type handlerType))
            {
                _logger.LogWarning($"Failed to find '{packetHeader}'");
                return Task.CompletedTask;
            }

            object handler = ActivatorUtilities.CreateInstance(_serviceProvider, handlerType);

            if (handler is not null)
            {
                Type packetType = packetHeader.FindType() ?? throw new InvalidOperationException($"Failed to find type '{packetHeader}'");
                string message = reader.BaseStream.IsEndOfStream() ? null : reader.ReadString();
                object messageObject = JsonSerializer.Deserialize(message, packetType);

                handler.InvokeMethod("Execute", this, messageObject);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"An error occured while parsing packet '{packetHeader}'.");
        }

        return Task.CompletedTask;
    }

    public void SendMessage(object message)
    {
        if (message is not null)
        {
            using BinaryWriter writer = new(new MemoryStream());
            writer.Write(message.GetType().Name);
            writer.Write(JsonSerializer.Serialize(message));

            base.Send(writer.BaseStream);
        }
    }
}