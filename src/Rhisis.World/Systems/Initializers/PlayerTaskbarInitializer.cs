using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Common;
using Rhisis.Core.DependencyInjection;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.World.Game;
using Rhisis.World.Game.Components;
using Rhisis.World.Game.Entities;
using Rhisis.World.Game.Structures;
using Rhisis.World.Systems.Taskbar;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.World.Systems.Initializers
{
    [Injectable(ServiceLifetime.Singleton)]
    public sealed class PlayerTaskbarInitializer : IGameSystemLifeCycle
    {
        private readonly IRhisisDatabase _database;
        private readonly ITaskbarSystem _taskbarSystem;

        public int Order => 1;

        public PlayerTaskbarInitializer(IRhisisDatabase database, ITaskbarSystem taskbarSystem)
        {
            _database = database;
            _taskbarSystem = taskbarSystem;
        }

        /// <inheritdoc />
        public void Initialize(IPlayerEntity player)
        {
            IEnumerable<DbShortcut> shortcuts = _database.TaskbarShortcuts.Where(x => x.CharacterId == player.PlayerData.Id).AsNoTracking().AsEnumerable();

            player.Taskbar.Applets = new TaskbarAppletContainerComponent(TaskbarSystem.MaxTaskbarApplets);
            player.Taskbar.Items = new TaskbarItemContainerComponent(TaskbarSystem.MaxTaskbarItems, TaskbarSystem.MaxTaskbarItemLevels);
            player.Taskbar.Queue = new TaskbarQueueContainerComponent(TaskbarSystem.MaxTaskbarQueue);

            foreach (DbShortcut dbShortcut in shortcuts)
            {
                Shortcut shortcut = null;

                if (dbShortcut.Type == ShortcutType.Item)
                {
                    Item item = player.Inventory.GetItemAtSlot(dbShortcut.ObjectItemSlot.GetValueOrDefault(-1));

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
                    _taskbarSystem.AddShortcutToAppletTaskbar(player, shortcut);
                }
                else if (dbShortcut.TargetTaskbar == ShortcutTaskbarTarget.Item)
                {
                    _taskbarSystem.AddShortcutToItemTaskbar(player, shortcut, dbShortcut.SlotLevelIndex);
                }
            }
        }

        /// <inheritdoc />
        public void Save(IPlayerEntity player)
        {
            DbCharacter character = _database.Characters
                .Include(x => x.TaskbarShortcuts)
                .FirstOrDefault(x => x.Id == player.PlayerData.Id);

            character.TaskbarShortcuts.Clear();

            _database.SaveChanges();

            foreach (Shortcut appletShortcut in player.Taskbar.Applets.Objects)
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
                    appletShortcut.Text);

                dbApplet.CharacterId = character.Id;

                if (appletShortcut.Type == ShortcutType.Item && appletShortcut.ItemIndex.HasValue)
                {
                    InventoryItem inventoryItem = player.Inventory.GetItemAtIndex(appletShortcut.ItemIndex.Value);

                    if (inventoryItem is null)
                    {
                        continue;
                    }

                    dbApplet.ObjectItemSlot = inventoryItem.Slot;
                }

                _database.TaskbarShortcuts.Add(dbApplet);
            }


            for (int slotLevel = 0; slotLevel < player.Taskbar.Items.Objects.Count; slotLevel++)
            {
                for (int slot = 0; slot < player.Taskbar.Items.Objects[slotLevel].Count; slot++)
                {
                    Shortcut itemShortcut = player.Taskbar.Items.Objects[slotLevel][slot];

                    if (itemShortcut == null)
                    {
                        continue;
                    }

                    var dbShortcut = new DbShortcut(ShortcutTaskbarTarget.Item, slotLevel, itemShortcut.Slot,
                        itemShortcut.Type, itemShortcut.ItemIndex, itemShortcut.ObjectType, itemShortcut.ObjectIndex,
                        itemShortcut.UserId, itemShortcut.ObjectData, itemShortcut.Text);

                    dbShortcut.CharacterId = character.Id;

                    if (itemShortcut.Type == ShortcutType.Item && itemShortcut.ItemIndex.HasValue)
                    {
                        Item item = player.Inventory.GetItemAtIndex(itemShortcut.ItemIndex.Value);

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
