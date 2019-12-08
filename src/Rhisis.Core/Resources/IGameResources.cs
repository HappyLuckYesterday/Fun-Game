﻿using Rhisis.Core.Structures.Game;
using Rhisis.Core.Structures.Game.Dialogs;
using Rhisis.Core.Structures.Game.Quests;
using System;
using System.Collections.Generic;

namespace Rhisis.Core.Resources
{
    /// <summary>
    /// Provides a mechanism to load data from resource files.
    /// </summary>
    public interface IGameResources
    {
        /// <summary>
        /// Gets the movers data.
        /// </summary>
        IReadOnlyDictionary<int, MoverData> Movers { get; }
        
        /// <summary>
        /// Gets the items data.
        /// </summary>
        IReadOnlyDictionary<int, ItemData> Items { get; }

        /// <summary>
        /// Gets the dialogs data.
        /// </summary>
        IReadOnlyDictionary<string, DialogSet> Dialogs { get; }

        /// <summary>
        /// Gets the shops data.
        /// </summary>
        IReadOnlyDictionary<string, ShopData> Shops { get; }

        /// <summary>
        /// Gets the jobs data.
        /// </summary>
        IReadOnlyDictionary<int, JobData> Jobs { get; }

        /// <summary>
        /// Gets the npcs data.
        /// </summary>
        IReadOnlyDictionary<string, NpcData> Npcs { get; }

        /// <summary>
        /// Gets the quests data.
        /// </summary>
        IReadOnlyDictionary<int, IQuestScript> Quests { get; }

        /// <summary>
        /// Gets the experience tables data.
        /// </summary>
        ExpTableData ExpTables { get; }

        /// <summary>
        /// Gets the penalities data.
        /// </summary>
        DeathPenalityData Penalities { get; }

        /// <summary>
        /// Load all loaders passed as parameter.
        /// </summary>
        /// <param name="loaders">Loader types.</param>
        void Load(params Type[] loaders);
    }
}
