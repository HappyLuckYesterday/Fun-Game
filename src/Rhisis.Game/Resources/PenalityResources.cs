using Microsoft.Extensions.Logging;
using Rhisis.Game.Common.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Rhisis.Game.Resources;

public sealed class PenalityResources
{
    private readonly ILogger<PenalityResources> _logger;

    private IEnumerable<PenalityProperties> _revivalPenality;
    private IEnumerable<PenalityProperties> _decExpPenality;
    private IEnumerable<PenalityProperties> _levelDownPenality;

    public PenalityResources(ILogger<PenalityResources> logger)
    {
        _logger = logger;
    }

    public void Load()
    {
        Stopwatch watch = new();
        watch.Start();
        string deathPenalityPath = GameResourcePaths.DeathPenalityPath;

        if (!File.Exists(deathPenalityPath))
        {
            _logger.LogWarning("Unable to load death penality. Reason: cannot find '{0}' file.", deathPenalityPath);
            return;
        }

        DeathPenalityProperties penalities = JsonSerializer.Deserialize<DeathPenalityProperties>(File.ReadAllText(deathPenalityPath), new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        if (penalities is null)
        {
            _logger.LogError("Failed to load death penalities.");
            return;
        }

        _revivalPenality = penalities.RevivalPenality.OrderBy(x => x.Level);
        _decExpPenality = penalities.DecExpPenality.OrderBy(x => x.Level);
        _levelDownPenality = penalities.LevelDownPenality.OrderBy(x => x.Level);

        watch.Stop();
        _logger.LogInformation($"Game penalities loaded in {watch.ElapsedMilliseconds}ms.");
    }


    /// <summary>
    /// Gets a revival penality by a level.
    /// </summary>
    /// <param name="level">Level.</param>
    /// <returns>Revival penality expressed as a percentage.</returns>
    public decimal GetRevivalPenality(int level) => GetPenality(_revivalPenality, level).Value;

    /// <summary>
    /// Gets the experience penality by a level.
    /// </summary>
    /// <param name="level">Level.</param>
    /// <returns>Experience penality expressed as a percentage.</returns>
    public decimal GetDecExpPenality(int level) => GetPenality(_decExpPenality, level).Value;

    /// <summary>
    /// Gets the level down penality by a level.
    /// </summary>
    /// <param name="level">Level</param>
    /// <returns>Boolean value that indicates if level should go down.</returns>
    public bool GetLevelDownPenality(int level) => Convert.ToBoolean(GetPenality(_levelDownPenality, level).Value);

    /// <summary>
    /// Gets a <see cref="PenalityProperties"/> from a collection of penalities and a level.
    /// </summary>
    /// <param name="penalityData">Penality data colletion.</param>
    /// <param name="level">Level</param>
    /// <returns></returns>
    private static PenalityProperties GetPenality(IEnumerable<PenalityProperties> penalityData, int level)
        => penalityData.FirstOrDefault(x => level <= x.Level) ?? penalityData.LastOrDefault();
}
