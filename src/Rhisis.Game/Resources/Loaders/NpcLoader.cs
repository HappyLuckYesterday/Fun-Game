using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Common.Resources;
using Rhisis.Game.Common.Resources.Dialogs;
using Rhisis.Game.IO.Include;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhisis.Game.Resources.Loaders
{
    public sealed class NpcLoader : IGameResourceLoader
    {
        private readonly ILogger<NpcLoader> _logger;
        private readonly IMemoryCache _cache;
        private readonly IGameResources _gameResources;
        private readonly WorldConfiguration _configuration;
        private readonly IDictionary<string, string> _texts;

        /// <summary>
        /// Creates a new <see cref="NpcLoader"/> instance.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="cache">Memory cache.</param>
        /// <param name="gameResources">Game resources.</param>
        /// <param name="worldConfiguration">World configuration.</param>
        public NpcLoader(ILogger<NpcLoader> logger, IMemoryCache cache, IGameResources gameResources, IOptions<WorldConfiguration> worldConfiguration)
        {
            _logger = logger;
            _cache = cache;
            _gameResources = gameResources;
            _configuration = worldConfiguration.Value;
            _texts = _cache.Get<IDictionary<string, string>>(GameResourcesConstants.Texts);
        }

        /// <inheritdoc />
        public void Load()
        {
            IEnumerable<string> files = Directory.GetFiles(GameResourcesConstants.Paths.ResourcePath, "character*.inc", SearchOption.AllDirectories);

            var npcData = new ConcurrentDictionary<string, NpcData>();

            foreach (var file in files)
            {
                using (var npcFile = new IncludeFile(file))
                {
                    foreach (IStatement npcStatement in npcFile.Statements)
                    {
                        if (!(npcStatement is Block npcBlock))
                        {
                            continue;
                        }

                        var npcId = npcStatement.Name;
                        var npcName = npcId;
                        var canBuff = false;

                        foreach (IStatement npcInfoStatement in npcBlock.Statements)
                        {
                            if (npcInfoStatement is Instruction instruction)
                            {
                                if (npcInfoStatement.Name == "SetName" && instruction.Parameters.Count > 0)
                                {
                                    npcName = _texts[instruction.Parameters.First().ToString()];
                                }
                                if (npcInfoStatement.Name == "AddMenu")
                                {
                                    canBuff = instruction.Parameters.FirstOrDefault() == "MMI_NPC_BUFF";
                                }
                            }
                        }
                        //TODO: implement other npc settings (image, music, actions...)
                        //      + constants for statement (like SetName)

                        _gameResources.Shops.TryGetValue(npcId, out ShopData shop);

                        DialogData dialogData = null;
                        if (_gameResources.Dialogs.TryGetValue(_configuration.Language, out DialogSet dialogSet))
                        {
                            dialogSet.TryGetValue(npcId, out dialogData);
                        }

                        var npc = new NpcData(npcId, npcName, shop, dialogData)
                        {
                            CanBuff = canBuff
                        };

                        if (npcData.ContainsKey(npc.Id))
                        {
                            npcData[npc.Id] = npc;
                            _logger.LogWarning(GameResourcesConstants.Errors.ObjectOverridedMessage, "NPC", npc.Id, "already declared");
                        }
                        else
                        {
                            npcData.TryAdd(npc.Id, npc);
                        }
                    }
                }
            }

            _cache.Set(GameResourcesConstants.Npcs, npcData);
            _logger.LogInformation($"-> {npcData.Count} NPCs loaded.");
        }
    }
}
