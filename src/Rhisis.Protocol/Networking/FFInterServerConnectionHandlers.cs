using Rhisis.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Protocol.Networking;

internal static class FFInterServerConnectionHandlers
{
    private static readonly Dictionary<string, Type> _handlers = new();

    static FFInterServerConnectionHandlers()
    {
        _handlers = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.FullName.StartsWith("Rhisis"))
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsClass && x.ImplementsInterface(typeof(IFFInterServerConnectionHandler<,>)))
            .ToDictionary(
                x => x.GetInterfaces()
                    .FirstOrDefault(x => x.IsInterface && x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IFFInterServerConnectionHandler<,>))?
                    .GetGenericArguments()
                    .Last().Name,
                x => x
            );
    }

    public static bool TryGetHandler(string handlerName, out Type handler)
    {
        return _handlers.TryGetValue(handlerName, out handler);
    }
}
