using Rhisis.Game.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Game.Resources;

public class GameResources
{
    private static readonly Lazy<GameResources> _currentInstance = new(() => new GameResources());

    /// <summary>
    /// Gets the current game resource instance.
    /// </summary>
    public static GameResources Current => _currentInstance.Value;

    private readonly ConcurrentDictionary<string, int> _defines = new();
    private readonly ConcurrentDictionary<int, ItemProperties> _itemsById = new();
    private readonly ConcurrentDictionary<string, ItemProperties> _itemsByIdentifierName = new();

    private GameResources()
    {
    }

    public void LoadDefines()
    {
        var headerFiles = from x in Directory.GetFiles(GameResourcePaths.ResourcePath, "*.*", SearchOption.AllDirectories)
                          where DefineFile.Extensions.Contains(Path.GetExtension(x))
                          select x;

        foreach (var headerFile in headerFiles)
        {
            using DefineFile defineFile = new(headerFile);
            
            foreach (var define in defineFile.Values)
            {
                var isIntValue = int.TryParse(define.Value.ToString(), out var intValue);

                if (isIntValue && !_defines.ContainsKey(define.Key))
                {
                    _defines.TryAdd(define.Key, intValue);
                }
            }
        }
    }

    public void LoadItems()
    {
        if (!File.Exists(GameResourcePaths.ItemsPropPath))
        {
            throw new FileNotFoundException($"Unable to load items. Reason: cannot find '{GameResourcePaths.ItemsPropPath}' file.");
        }

        using ResourceTableFile propItem = new(GameResourcePaths.ItemsPropPath, headerLineIndex: 1, _defines);
        
        var items = propItem.GetRecords<ItemProperties>();

        foreach (ItemProperties item in items)
        {
            int itemId = _defines.GetValueOrDefault(item.IdentifierName);

            if (itemId <= 0)
            {
                continue;
            }

            item.Id = itemId;

            if (!_itemsById.TryAdd(item.Id, item))
            {
                Console.WriteLine($"Failed to add item: {item.IdentifierName} ({item.Name}). Item already exists.");
            }

            if (!_itemsByIdentifierName.TryAdd(item.IdentifierName, item))
            {
                Console.WriteLine($"Failed to add item: {item.IdentifierName} ({item.Name}). Item already exists.");
            }
        }
    }

    public ItemProperties GetItem(string itemIdentifier)
    {
        if (int.TryParse(itemIdentifier, out int itemId))
        {
            return GetItem(itemId);
        }
        else
        {
            return _itemsByIdentifierName.TryGetValue(itemIdentifier, out ItemProperties item) ? item : null;
        }
    }

    public ItemProperties GetItem(int itemId) => _itemsById.TryGetValue(itemId, out ItemProperties item) ? item : null;


}
