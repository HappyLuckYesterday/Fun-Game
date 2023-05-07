using Microsoft.Extensions.Logging;
using Rhisis.Game.IO;
using Rhisis.Game.Resources.Properties;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Rhisis.Game.Resources;

public sealed class ItemResources
{
    private readonly ILogger<ItemResources> _logger;
    private readonly ConcurrentDictionary<string, int> _defines;
    private readonly ConcurrentDictionary<int, ItemProperties> _itemsById = new();
    private readonly ConcurrentDictionary<string, ItemProperties> _itemsByIdentifierName = new();

    internal ItemResources(ILogger<ItemResources> logger, ConcurrentDictionary<string, int> defines)
    {
        _logger = logger;
        _defines = defines;
    }

    public ItemProperties Get(string itemIdentifier)
    {
        if (int.TryParse(itemIdentifier, out int itemId))
        {
            return Get(itemId);
        }
        else
        {
            return _itemsByIdentifierName.TryGetValue(itemIdentifier, out ItemProperties item) ? item : null;
        }
    }

    public ItemProperties Get(int itemId) => _itemsById.TryGetValue(itemId, out ItemProperties item) ? item : null;

    public IEnumerable<ItemProperties> Where(Func<ItemProperties, bool> predicate) => _itemsById.Values.Where(predicate);

    public void Load()
    {
        Stopwatch watch = new();
        watch.Start();

        if (!File.Exists(GameResourcePaths.ItemsPropPath))
        {
            throw new FileNotFoundException($"Unable to load items. Reason: cannot find '{GameResourcePaths.ItemsPropPath}' file.");
        }

        using ResourceTableFile propItem = new(GameResourcePaths.ItemsPropPath, headerLineIndex: 1, _defines);

        IEnumerable<ItemProperties> items = propItem.GetRecords<ItemProperties>();

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
                _logger.LogWarning($"Failed to add item: {item.IdentifierName} ({item.Name}). Item already exists.");
            }

            if (!_itemsByIdentifierName.TryAdd(item.IdentifierName, item))
            {
                _logger.LogWarning($"Failed to add item: {item.IdentifierName} ({item.Name}). Item already exists.");
            }
        }

        watch.Stop();
        _logger.LogInformation($"{_itemsById.Count} items loaded in {watch.ElapsedMilliseconds}ms.");
    }
}