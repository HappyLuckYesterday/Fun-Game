using Microsoft.Extensions.Logging;
using Rhisis.Game.IO.Include;
using Rhisis.Game.Resources.Properties;
using Rhisis.Game.Resources.Properties.Dialogs;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Rhisis.Game.Resources;

public sealed class NpcResources
{
    private readonly ILogger<NpcResources> _logger;
    private readonly ConcurrentDictionary<string, NpcProperties> _npcs = new();

    public NpcResources(ILogger<NpcResources> logger)
    {
        _logger = logger;
    }

    public NpcProperties Get(string npcName) => _npcs.TryGetValue(npcName, out NpcProperties npc) ? npc : null;

    public void Load()
    {
        Stopwatch watch = new();
        watch.Start();

        IEnumerable<string> files = Directory.GetFiles(GameResourcePaths.ResourcePath, "character*.inc", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            using IncludeFile npcFile = new(file);

            foreach (IStatement npcStatement in npcFile.Statements)
            {
                if (npcStatement is not Block npcBlock)
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
                            npcName = instruction.Parameters.First().ToString();
                        }
                        if (npcInfoStatement.Name == "AddMenu")
                        {
                            canBuff = instruction.Parameters.FirstOrDefault()?.ToString().Equals("MMI_NPC_BUFF") ?? false;
                        }
                    }
                }
                //TODO: implement other npc settings (image, music, actions...)
                //      + constants for statement (like SetName)

                string shopFilePath = Path.Combine(GameResourcePaths.ShopsPath, $"{npcId}.json");
                ShopProperties npcShop = null;

                if (File.Exists(shopFilePath))
                {
                    npcShop = JsonSerializer.Deserialize<ShopProperties>(File.ReadAllText(shopFilePath));
                }

                string dialogFilePath = Path.Combine(GameResourcePaths.DialogsPath, "en", $"{npcId}.json");
                DialogProperties npcDialog = null;

                if (File.Exists(dialogFilePath))
                {
                    npcDialog = JsonSerializer.Deserialize<DialogProperties>(File.ReadAllText(dialogFilePath));
                }

                var npc = new NpcProperties(npcId, npcName, npcShop, npcDialog)
                {
                    CanBuff = canBuff
                };

                if (!_npcs.TryAdd(npc.Id, npc))
                {
                    _logger.LogWarning($"Cannot add npc '{npc.Id}'. Reason: Already exist.");
                }
            }
        }

        watch.Stop();
        _logger.LogInformation($"{_npcs.Count} npcs loaded in {watch.ElapsedMilliseconds}ms.");
    }
}
