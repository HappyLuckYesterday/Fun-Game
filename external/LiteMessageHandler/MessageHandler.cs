using LiteMessageHandler.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LiteMessageHandler;

public class MessageHandler : IDisposable
{
    private readonly IServiceScope? _serviceScope;
    private readonly MessageHandlerExecutor _executor;

    public object Target { get; }

    internal MessageHandler(MessageHandlerAction? handlerAction, IServiceProvider? serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(handlerAction);
        ArgumentNullException.ThrowIfNull(handlerAction.Executor);

        _serviceScope = serviceProvider?.CreateScope();
        _executor = handlerAction.Executor;
        Target = handlerAction.CreateInstance(_serviceScope?.ServiceProvider)!;
    }

    public void Execute(object parameter)
    {
        _executor.Execute(Target, parameter);
    }

    public void Dispose()
    {
        _serviceScope?.Dispose();
    }
}
