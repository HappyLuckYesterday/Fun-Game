using LiteMessageHandler.Extensions;
using LiteMessageHandler.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteMessageHandler;

public class MessageHandlerDispatcher : IMessageHandlerDispatcher
{
    private readonly IServiceProvider? _serviceProvider;
    private readonly MessageHandlerActionCache _handlerCache;

    public MessageHandlerDispatcher(IServiceProvider? serviceProvider = null)
    {
        _serviceProvider = serviceProvider;

        Dictionary<Type, MessageHandlerAction>? handlers = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.ImplementsInterface(typeof(IMessageHandler<>)))
            .Select(x => new MessageHandlerAction(x))
            .ToDictionary(keySelector: x => x.HandlerParameterType, elementSelector: x => x);

        _handlerCache = new MessageHandlerActionCache(handlers);
    }

    public MessageHandler? GetHandler(Type? handlerType)
    {
        MessageHandlerAction? handler = _handlerCache.GetMessageHandler(handlerType);

        if (handler == null)
        {
            return null;
        }

        return new MessageHandler(handler.CreateInstance(_serviceProvider), handler.Executor);
    }

    public MessageHandler? GetHandler<TMessage>()
    {
        return GetHandler(typeof(TMessage));
    }

    public void Dispatch<TMessage>(TMessage message)
    {
        MessageHandler? handler = GetHandler(typeof(TMessage));

        if (handler == null)
        {
            throw new InvalidOperationException($"Cannot find handler for type '{typeof(TMessage).FullName}'.");
        }

        handler.Execute(message!);
    }

    public void Dispose()
    {
        _handlerCache?.Dispose();
    }
}
