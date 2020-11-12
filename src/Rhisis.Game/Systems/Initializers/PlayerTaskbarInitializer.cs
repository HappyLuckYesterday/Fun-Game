using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Common;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Game.Systems.Initializers
{
    [Injectable]
    public sealed class PlayerTaskbarInitializer : IPlayerInitializer
    {
        private readonly IRhisisDatabase _database;

        public int Order => 1;

        public PlayerTaskbarInitializer(IRhisisDatabase database)
        {
            _database = database;
        }

        /// <inheritdoc />
        public void Load(IPlayer player)
        {
            IEnumerable<DbShortcut> shortcuts = _database.TaskbarShortcuts
                .Where(x => x.CharacterId == player.CharacterId)
                .AsNoTracking()
                .AsEnumerable();

            foreach (DbShortcut dbShortcut in shortcuts)
            {
                IShortcut shortcut = null;

                if (dbShortcut.Type == ShortcutType.Item)
                {
                    IItem item = player.Inventory.GetItem(x => x.Slot == dbShortcut.ObjectItemSlot.GetValueOrDefault(-1));

                    if (item is null)
                    {
                        return;
                    }

                    shortcut = new Shortcut(dbShortcut.Slot, dbShortcut.Type, item.Index, dbShortcut.ObjectType, dbShortcut.ObjectIndex, dbShortcut.UserId, dbShortcut.ObjectData, dbShortcut.Text);
                }
                else
                {
                    shortcut = new Shortcut(dbShortcut.Slot, dbShortcut.Type, dbShortcut.ObjectItemSlot, dbShortcut.ObjectType, dbShortcut.ObjectIndex, dbShortcut.UserId, dbShortcut.ObjectData, dbShortcut.Text);
                }

                if (dbShortcut.TargetTaskbar == ShortcutTaskbarTarget.Applet)
                {
                    player.Taskbar.Applets.Add(shortcut, shortcut.Slot);
                }
                else if (dbShortcut.TargetTaskbar == ShortcutTaskbarTarget.Item)
                {
                    ITaskbarContainer<IShortcut> taskbarContainer = player.Taskbar.Items.GetContainerAtLevel(dbShortcut.SlotLevelIndex);

                    if (taskbarContainer != null)
                    {
                        taskbarContainer.Add(shortcut, shortcut.Slot);
                    }
                }
            }
        }

        /// <inheritdoc />
        public void Save(IPlayer player)
        {
            DbCharacter character = _database.Characters
                .Include(x => x.TaskbarShortcuts)
                .FirstOrDefault(x => x.Id == player.CharacterId);

            character.TaskbarShortcuts.Clear();

            _database.SaveChanges();

            foreach (IShortcut appletShortcut in player.Taskbar.Applets)
            {
                if (appletShortcut == null)
                {
                    continue;
                }

                var dbApplet = new DbShortcut(ShortcutTaskbarTarget.Applet,
                    appletShortcut.Slot,
                    appletShortcut.Type,
                    appletShortcut.ItemIndex,
                    appletShortcut.ObjectType,
                    appletShortcut.ObjectIndex,
                    appletShortcut.UserId,
                    appletShortcut.ObjectData,
                    appletShortcut.Text)
                {
                    CharacterId = character.Id
                };

                if (appletShortcut.Type == ShortcutType.Item && appletShortcut.ItemIndex.HasValue)
                {
                    IItem inventoryItem = player.Inventory.GetItem(appletShortcut.ItemIndex.Value);

                    if (inventoryItem is null)
                    {
                        continue;
                    }

                    dbApplet.ObjectItemSlot = inventoryItem.Slot;
                }

                _database.TaskbarShortcuts.Add(dbApplet);
            }


            for (var level = 0; level < player.Taskbar.Items.Count(); level++)
            {
                ITaskbarContainer<IShortcut> taskbarContainer = player.Taskbar.Items.GetContainerAtLevel(level);

                foreach (IShortcut itemShortcut in taskbarContainer)
                {
                    if (itemShortcut == null)
                    {
                        continue;
                    }

                    var dbShortcut = new DbShortcut(ShortcutTaskbarTarget.Item, level, itemShortcut.Slot,
                        itemShortcut.Type, itemShortcut.ItemIndex, itemShortcut.ObjectType, itemShortcut.ObjectIndex,
                        itemShortcut.UserId, itemShortcut.ObjectData, itemShortcut.Text)
                    {
                        CharacterId = character.Id
                    };

                    if (itemShortcut.Type == ShortcutType.Item && itemShortcut.ItemIndex.HasValue)
                    {
                        IItem item = player.Inventory.GetItem(itemShortcut.ItemIndex.Value);

                        if (item is null)
                        {
                            continue;
                        }

                        dbShortcut.ObjectItemSlot = item.Slot;
                    }

                    _database.TaskbarShortcuts.Add(dbShortcut);
                }
            }

            _database.SaveChanges();
        }
    }
}
