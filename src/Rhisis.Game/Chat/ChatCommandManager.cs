using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core;
using Rhisis.Core.Helpers;
using Rhisis.Game.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhisis.Game.Chat;

public sealed class ChatCommandManager : Singleton<ChatCommandManager>
{
    private readonly ConcurrentDictionary<string, ChatCommandDefinition> _chatCommands = new();

    private IServiceProvider _serviceProvider;
    private ILogger<ChatCommandManager> _logger;

    public void Load(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = _serviceProvider.GetRequiredService<ILogger<ChatCommandManager>>();

        _logger.LogInformation($"Loading chat commands...");

        IEnumerable<TypeInfo> chatCommandTypes = ReflectionHelper.GetClassesAssignableFrom<IChatCommand>();

        foreach (TypeInfo chatCommandType in chatCommandTypes)
        {
            IEnumerable<ChatCommandAttribute> attributes = chatCommandType.GetCustomAttributes<ChatCommandAttribute>();

            if (attributes.Any())
            {
                ObjectFactory chatCommandFactory = ActivatorUtilities.CreateFactory(chatCommandType, Type.EmptyTypes);

                foreach (ChatCommandAttribute chatAttribute in attributes)
                {
                    var chatCommandDefinition = new ChatCommandDefinition(chatAttribute.Command, chatCommandFactory, chatAttribute.MinimumAuthorization);

                    if (!_chatCommands.TryAdd(chatAttribute.Command, chatCommandDefinition))
                    {
                        throw new InvalidOperationException($"Duplicate chat command: '{chatAttribute.Command}' already exists.");
                    }
                }
            }
        }

        _logger.LogInformation($"{_chatCommands.Count} chat commands loaded.");
    }

    public IChatCommand Get(string command, AuthorityType authority)
    {
        if (_chatCommands.TryGetValue(command, out ChatCommandDefinition chatCommandDefinition))
        {
            if (authority < chatCommandDefinition.Authority)
            {
                throw new InvalidOperationException($"Player doesn't have enough authority to execute this command.");
            }

            return chatCommandDefinition.ChatCommandFactory(_serviceProvider, null) as IChatCommand;
        }
        else
        {
            throw new KeyNotFoundException($"Cannot find chat command: '{command}'.");
        }
    }
}
