using Microsoft.Extensions.Logging;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.IO;
using Rhisis.Game.IO.Include;
using Rhisis.Game.Resources.Properties;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Rhisis.Game.Resources;

public sealed class MoverResources
{
    private readonly ILogger<MoverResources> _logger;
    private readonly ConcurrentDictionary<string, int> _defines;
    private readonly ConcurrentDictionary<int, MoverProperties> _moversById = new();
    private readonly ConcurrentDictionary<string, MoverProperties> _moversByIdentifierName = new();

    internal MoverResources(ILogger<MoverResources> logger, ConcurrentDictionary<string, int> defines)
    {
        _logger = logger;
        _defines = defines;
    }

    public MoverProperties Get(int moverId) => _moversById.GetValueOrDefault(moverId);

    public MoverProperties Get(string moverIdentifier)
    {
        if (int.TryParse(moverIdentifier, out int moverId))
        {
            return Get(moverId);
        }
        else
        {
            return _moversByIdentifierName.TryGetValue(moverIdentifier, out MoverProperties mover) ? mover : null;
        }
    }

    public void Load()
    {
        Stopwatch watch = new();
        watch.Start();
        if (!File.Exists(GameResourcePaths.MoversPropPath))
        {
            throw new FileNotFoundException($"Unable to load mover properties. Reason: cannot find '{GameResourcePaths.MoversPropPath}' file.");
        }
        if (!File.Exists(GameResourcePaths.MoversPropExPath))
        {
            throw new FileNotFoundException($"Unable to load extended mover properties. Reason: cannot find '{GameResourcePaths.MoversPropExPath}' file.");
        }

        using ResourceTableFile propMover = new(GameResourcePaths.MoversPropPath, headerLineIndex: 1, defines: _defines);
        IEnumerable<MoverProperties> movers = propMover.GetRecords<MoverProperties>();

        foreach (MoverProperties mover in movers)
        {
            int moverId = _defines.GetValueOrDefault(mover.IdentifierName);

            if (moverId <= 0)
            {
                continue;
            }

            mover.Id = moverId;

            if (!_moversById.TryAdd(mover.Id, mover))
            {
                _logger.LogWarning($"Failed to add mover: {mover.IdentifierName} ({mover.Name}). Mover already exists.");
            }

            if (!_moversByIdentifierName.TryAdd(mover.IdentifierName, mover))
            {
                _logger.LogWarning($"Failed to add mover: {mover.IdentifierName} ({mover.Name}). Mover already exists.");
            }
        }


        using IncludeFile moversPropExFile = new(GameResourcePaths.MoversPropExPath);
        foreach (IStatement statement in moversPropExFile.Statements)
        {
            if (statement is Block moverBlock)
            {
                if (_defines.TryGetValue(moverBlock.Name, out var moverId) && _moversById.TryGetValue(moverId, out MoverProperties mover))
                {
                    LoadDropGold(mover, moverBlock.GetInstruction("DropGold"));
                    LoadDropItems(mover, moverBlock.GetInstructions("DropItem"));
                    LoadDropItemsKind(mover, moverBlock.GetInstructions("DropKind"));

                    Variable maxDropVariable = moverBlock.GetVariable("Maxitem");
                    
                    if (maxDropVariable is null)
                    {
                        continue;
                    }

                    mover.MaxDropItem = int.Parse(maxDropVariable.Value.ToString());
                }
            }
        }

        watch.Stop();
        _logger.LogInformation($"{_moversById.Count} movers loaded in {watch.ElapsedMilliseconds}ms.");
    }

    /// <summary>
    /// Loads the DropGold instruction for a given mover.
    /// </summary>
    /// <param name="mover">Mover</param>
    /// <param name="dropGoldInstruction">DropGold instruction</param>
    private void LoadDropGold(MoverProperties mover, Instruction dropGoldInstruction)
    {
        if (dropGoldInstruction is null)
        {
            return;
        }

        if (dropGoldInstruction.Parameters.Count < 2)
        {
            _logger.LogWarning($"Cannot load 'DropGold' instruction for mover {mover.Name}. Reason: Missing parameters.");
            return;
        }

        if (!int.TryParse(dropGoldInstruction.Parameters.ElementAt(0).ToString(), out var minGold))
        {
            _logger.LogWarning($"Cannot load min gold amount for mover {mover.Name}.");
        }

        if (!int.TryParse(dropGoldInstruction.Parameters.ElementAt(1).ToString(), out var maxGold))
        {
            _logger.LogWarning($"Cannot load max gold amount for mover {mover.Name}.");
        }

        mover.DropGoldMin = minGold;
        mover.DropGoldMax = maxGold;
    }

    /// <summary>
    /// Loads a collection of DropItem instruction for a given mover.
    /// </summary>
    /// <param name="mover">Mover</param>
    /// <param name="dropItemInstructions">Collection of DropItem instructions</param>
    private void LoadDropItems(MoverProperties mover, IEnumerable<Instruction> dropItemInstructions)
    {
        if (dropItemInstructions is null)
        {
            return;
        }

        foreach (var dropItemInstruction in dropItemInstructions)
        {
            DropItemProperties dropItem = new();
            string dropItemName = dropItemInstruction.Parameters.ElementAt(0).ToString();

            if (_defines.TryGetValue(dropItemName, out var itemId))
            {
                dropItem.ItemId = itemId;
            }
            else
            {
                _logger.LogWarning($"Cannot find drop item id: {dropItemName} for mover {mover.Name}.");
                continue;
            }

            if (long.TryParse(dropItemInstruction.Parameters.ElementAt(1).ToString(), out var probability))
            {
                dropItem.Probability = probability;
            }
            else
            {
                _logger.LogWarning($"Cannot read drop item probability for item {dropItemName} and mover {mover.Name}.");
            }

            if (int.TryParse(dropItemInstruction.Parameters.ElementAt(2).ToString(), out var itemMaxRefine))
            {
                dropItem.ItemMaxRefine = itemMaxRefine;
            }
            else
            {
                _logger.LogWarning($"Cannot read drop item refine max for item {dropItemName} and mover {mover.Name}.");
            }

            if (int.TryParse(dropItemInstruction.Parameters.ElementAt(3).ToString(), out var itemCount))
            {
                dropItem.Count = itemCount;
            }
            else
            {
                _logger.LogWarning($"Cannot read drop item count for item {dropItemName} and mover {mover.Name}.");
            }

            mover.DropItems.Add(dropItem);
        }
    }

    /// <summary>
    /// Loads a collection of DropKind instructions for a given mover.
    /// </summary>
    /// <param name="mover"></param>
    /// <param name="instructions"></param>
    private void LoadDropItemsKind(MoverProperties mover, IEnumerable<Instruction> instructions)
    {
        if (instructions is null)
        {
            return;
        }

        foreach (var dropItemKindInstruction in instructions)
        {
            DropItemKindProperties dropItemKind = new();

            if (dropItemKindInstruction.Parameters.Count < 0 || dropItemKindInstruction.Parameters.Count > 3)
            {
                _logger.LogWarning($"Cannot load 'DropKind' instruction for mover {mover.Name}. Reason: Missing parameters.");
                continue;
            }

            var itemKind = dropItemKindInstruction.Parameters.ElementAt(0).ToString().Replace("IK3_", string.Empty);
            dropItemKind.ItemKind = (ItemKind3)Enum.Parse(typeof(ItemKind3), itemKind);

            // From official files: Project.cpp:2824
            dropItemKind.UniqueMin = Math.Max(mover.Level - 5, 1);
            dropItemKind.UniqueMax = Math.Max(mover.Level - 2, 1);

            mover.DropItemsKind.Add(dropItemKind);
        }
    }
}
