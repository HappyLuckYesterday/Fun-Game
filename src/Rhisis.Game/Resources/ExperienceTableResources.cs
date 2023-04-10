using Microsoft.Extensions.Logging;
using Rhisis.Core.Extensions;
using Rhisis.Game.IO.Include;
using Rhisis.Game.Resources.Properties;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Rhisis.Game.Resources;

public sealed class ExperienceTableResources
{
    private readonly ILogger<ExperienceTableResources> _logger;

    private IEnumerable<long[]> _expDropLuck = new List<long[]>();
    private IReadOnlyDictionary<int, CharacterExpTableProperties> _characterExpTable;

    public ExperienceTableResources(ILogger<ExperienceTableResources> logger)
    {
        _logger = logger;
    }

    public void Load()
    {
        Stopwatch watch = new();
        watch.Start();
        string expTablePath = GameResourcePaths.ExpTablePath;

        if (!File.Exists(expTablePath))
        {
            _logger.LogWarning("Unable to load exp table. Reason: Cannot find '{0}' file.", expTablePath);
            return;
        }

        using (IncludeFile expTableFile = new(expTablePath, @"([(){}=,;\n\r\t ])"))
        {
            Block dropLuckBlock = expTableFile.GetBlock("expDropLuck");
            if (dropLuckBlock is null)
            {
                _logger.LogWarning("Unable to load exp table. Reason: Cannot find drop luck data.");
                return;
            }

            Block expCharacterBlock = expTableFile.GetBlock("expCharacter");
            if (expCharacterBlock is null)
            {
                _logger.LogWarning("Unable to load exp table. Reason: Cannot find character experience data.");
                return;
            }

            _expDropLuck = LoadDropLuck(dropLuckBlock);
            _characterExpTable = LoadCharacterExperience(expCharacterBlock);
        }

        watch.Stop();
        _logger.LogInformation($"Experience tables loaded in {watch.ElapsedMilliseconds}ms.");
    }

    /// <summary>
    /// Gets a drop luck by level and refine.
    /// </summary>
    /// <param name="level">Level</param>
    /// <param name="refine">Refine</param>
    /// <returns></returns>
    public long GetDropLuck(int level, int refine) => _expDropLuck.ElementAt(level - 1).ElementAt(refine);

    /// <summary>
    /// Gets a character experience information based on a level.
    /// </summary>
    /// <param name="level">Character level.</param>
    /// <returns><see cref="CharacterExpTableProperties"/> matching the level.</returns>
    public CharacterExpTableProperties GetCharacterExp(int level) => _characterExpTable.TryGetValue(level, out CharacterExpTableProperties value) ? value : default;

    /// <summary>
    /// Load a collection of item drop luck from a "expDropLuck" block instruction.
    /// </summary>
    /// <param name="dropLuckBlock"></param>
    private static IEnumerable<long[]> LoadDropLuck(Block dropLuckBlock)
        => dropLuckBlock.UnknownStatements.Select(x => long.Parse(x)).GroupBy(11).Select(x => x.ToArray());

    /// <summary>
    /// Loads the character experience table.
    /// </summary>
    /// <param name="expTableBlock"></param>
    private static IReadOnlyDictionary<int, CharacterExpTableProperties> LoadCharacterExperience(Block expTableBlock)
    {
        return expTableBlock.UnknownStatements
            .Select((value, index) => new { Value = value, Index = index })
            .GroupBy(item => item.Index / 4, item => item.Value)
            .Select(x =>
            {
                return new CharacterExpTableProperties(x.Key,
                    long.Parse(x.ElementAt(0)),
                    long.Parse(x.ElementAt(1)),
                    long.Parse(x.ElementAt(2)),
                    long.Parse(x.ElementAt(3)));
            }).ToDictionary(x => x.Level);
    }
}
