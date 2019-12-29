﻿using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rhisis.Core.IO;
using Rhisis.Core.Structures.Game;
using Rhisis.Core.Structures.Game.Dialogs;
using Rhisis.Core.Structures.Game.Quests;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rhisis.Core.Resources
{
    internal class GameResources : IGameResources
    {
        private readonly ILogger<GameResources> _logger;
        private readonly IMemoryCache _cache;
        private readonly IServiceProvider _serciceProvider;
        private ConcurrentDictionary<string, int> _defines;
        private ConcurrentDictionary<string, string> _texts;
        private ConcurrentDictionary<int, MoverData> _movers;
        private ConcurrentDictionary<int, ItemData> _items;
        private ConcurrentDictionary<string, DialogSet> _dialogs;
        private ConcurrentDictionary<string, ShopData> _shops;
        private ConcurrentDictionary<int, JobData> _jobs;
        private ConcurrentDictionary<string, NpcData> _npcs;
        private ConcurrentDictionary<int, IQuestScript> _quests;
        private ExpTableData _expTableData;
        private DeathPenalityData _penalities;

        /// <inheritdoc />
        public IReadOnlyDictionary<string, int> Defines => this.GetCacheValue(GameResourcesConstants.Defines, ref this._defines);

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Texts => this.GetCacheValue(GameResourcesConstants.Texts, ref this._texts);

        /// <inheritdoc />
        public IReadOnlyDictionary<int, MoverData> Movers => this.GetCacheValue(GameResourcesConstants.Movers, ref this._movers);

        /// <inheritdoc />
        public IReadOnlyDictionary<int, ItemData> Items => this.GetCacheValue(GameResourcesConstants.Items, ref this._items);

        /// <inheritdoc />
        public IReadOnlyDictionary<string, DialogSet> Dialogs => this.GetCacheValue(GameResourcesConstants.Dialogs, ref this._dialogs);

        /// <inheritdoc />
        public IReadOnlyDictionary<string, ShopData> Shops => this.GetCacheValue(GameResourcesConstants.Shops, ref this._shops);

        /// <inheritdoc />
        public IReadOnlyDictionary<int, JobData> Jobs => this.GetCacheValue(GameResourcesConstants.Jobs, ref this._jobs);

        /// <inheritdoc />
        public IReadOnlyDictionary<string, NpcData> Npcs => this.GetCacheValue(GameResourcesConstants.Npcs, ref this._npcs);

        /// <inheritdoc />
        public IReadOnlyDictionary<int, IQuestScript> Quests => this.GetCacheValue(GameResourcesConstants.Quests, ref this._quests);

        /// <inheritdoc />
        public ExpTableData ExpTables => this.GetCacheValue(GameResourcesConstants.ExpTables, ref this._expTableData);

        /// <inheritdoc />
        public DeathPenalityData Penalities => this.GetCacheValue(GameResourcesConstants.PenalityData, ref this._penalities);

        /// <summary>
        /// Creates a new <see cref="GameResources"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="cache">Memory cache.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public GameResources(ILogger<GameResources> logger, IMemoryCache cache, IServiceProvider serviceProvider)
        {
            this._logger = logger;
            this._cache = cache;
            this._serciceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public void Load(params Type[] loaders)
        {
            Profiler.Start("LoadResources");
            this._logger.LogInformation("Loading server resources...");

            foreach (Type loaderType in loaders)
            {
                var loader = (IGameResourceLoader)ActivatorUtilities.CreateInstance(this._serciceProvider, loaderType);

                if (loader != null)
                {
                    loader.Load();
                }
            }

            this._logger.LogInformation("Resources loaded in {0} ms.", Profiler.Stop("LoadResources").ElapsedMilliseconds);
        }

        /// <summary>
        /// Gets a cached value by it's key.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="key">Cached object key.</param>
        /// <param name="value">Object reference to store the cached value.</param>
        /// <returns></returns>
        private T GetCacheValue<T>(object key, ref T value)
        {
            if (Equals(value, default(T)))
            {
                this._cache.TryGetValue(key, out value);
            }

            return value;
        }

        /// <inheritdoc />
        public string GetText(string textKey, string defaultText = null)
        {
            if (this.Texts.TryGetValue(textKey, out string text))
            {
                return text;
            }

            return string.IsNullOrEmpty(defaultText) ? textKey : defaultText;
        }

        /// <inheritdoc />
        public int GetDefinedValue(string defineKey, int defaultValue = 0)
        {
            if (string.IsNullOrEmpty(defineKey))
            {
                return defaultValue;
            }

            return this.Defines.TryGetValue(defineKey, out int value) ? value : defaultValue;
        }
    }
}
