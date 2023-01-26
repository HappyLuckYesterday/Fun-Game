using LiteMessageHandler.Internal;
using System;

namespace LiteMessageHandler;

public class MessageHandler
{
    private readonly MessageHandlerExecutor _executor;

    public object Target { get; }

    internal MessageHandler(object? target, MessageHandlerExecutor? executor)
    {
        Target = target ?? throw new ArgumentNullException(nameof(target));
        _executor = executor ?? throw new ArgumentNullException(nameof(executor));
    }

    public void Execute(object parameter)
    {
        _executor.Execute(Target, parameter);
    }
}
