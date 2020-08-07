using Microsoft.Extensions.Logging;
using Rhisis.Core.Common;
using Rhisis.Core.DependencyInjection;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using System;

namespace Rhisis.World.Systems.Taskbar
{
    [Injectable]
    public sealed class TaskbarSystem : ITaskbarSystem
    {
        public const int MaxTaskbarApplets = 18;
        public const int MaxTaskbarItems = 9;
        public const int MaxTaskbarItemLevels = 8;
        public const int MaxTaskbarQueue = 5;

        private readonly ILogger<TaskbarSystem> _logger;

        /// <inheritdoc />
        public int Order => 1;

        /// <summary>
        /// Creates a new <see cref="TaskbarSystem"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="database">Rhisis database access layer.</param>
        public TaskbarSystem(ILogger<TaskbarSystem> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public void AddShortcutToAppletTaskbar(IPlayerEntity player, Shortcut shortcutToAdd)
        {
            if (player.Taskbar.Applets.Add(shortcutToAdd, shortcutToAdd.Slot))
            {
                _logger.LogTrace("Created Shortcut of type {0} on slot {1} for player {2} on the Applet Taskbar", 
                    Enum.GetName(typeof(ShortcutType), shortcutToAdd.Type), shortcutToAdd.Slot, player.Object.Name);
            }
            else
            {
                _logger.LogWarning($"Failed to add shortcut '{shortcutToAdd.Type}' to player's '{player}' applet taskbar.");
            }
        }

        /// <inheritdoc />
        public void AddShortcutToItemTaskbar(IPlayerEntity player, Shortcut shortcutToAdd, int slotLevelIndex)
        {
            if (player.Taskbar.Items.Add(shortcutToAdd, slotLevelIndex, shortcutToAdd.Slot))
            {
                _logger.LogTrace($"Created shortcut on item taskbar at level {slotLevelIndex} and slot {shortcutToAdd.Slot} for player {player}.");
            }
            else
            {
                _logger.LogWarning($"Failed to add shortcut '{shortcutToAdd.Type}' to player '{player}' at level {slotLevelIndex} and slot {shortcutToAdd.Slot} on item taskbar.");
            }
        }

        /// <inheritdoc />
        public void RemoveShortcutFromAppletTaskbar(IPlayerEntity player, int slotIndex)
        {
            if (player.Taskbar.Applets.RemoveAt(slotIndex))
            {
                _logger.LogTrace($"Shortcut removed from slot {slotIndex} on the applet taskbar of player '{player}'.");
            }
            else
            {
                _logger.LogWarning($"Failed to remove shortcut from slot {slotIndex} on applet taskbar of player '{player}'.");
            }
        }

        /// <inheritdoc />
        public void RemoveShortcutFromItemTaskbar(IPlayerEntity player, int slotLevelIndex, int slotIndex)
        {
            if (player.Taskbar.Items.RemoveAt(slotLevelIndex, slotIndex))
            {
                _logger.LogTrace($"Shortcut removed from level {slotLevelIndex} and slot {slotIndex} on items taskbar of player '{player}'.");
            }
            else
            {
                _logger.LogWarning($"Failed to remove shortcut from level {slotLevelIndex} and slot {slotIndex} on items taskbar of player '{player}'.");
            }
        }
    }
}