using Microsoft.Extensions.Logging;
using Rhisis.Core.Extensions;
using Rhisis.Game.IO;
using Rhisis.Game.Resources.Properties;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Rhisis.Game.Resources;

public sealed class SkillResources
{
    private readonly ILogger<SkillResources> _logger;
    private readonly ConcurrentDictionary<string, int> _defines;
    private readonly ConcurrentDictionary<int, SkillProperties> _skillsById = new();

    public SkillResources(ILogger<SkillResources> logger, ConcurrentDictionary<string, int> defines)
    {
        _logger = logger;
        _defines = defines;
    }

    public SkillProperties Get(int skillId) => _skillsById.GetValueOrDefault(skillId);

    public void Load()
    {
        Stopwatch watch = new();
        watch.Start();
        var propSkillPath = GameResourcePaths.SkillPropPath;
        var propSkillAddPath = GameResourcePaths.SkillPropAddPath;

        if (!File.Exists(propSkillPath))
        {
            _logger.LogWarning("Unable to load skills. Reason: cannot find '{0}' file.", propSkillPath);
            return;
        }

        if (!File.Exists(propSkillAddPath))
        {
            _logger.LogWarning("Unable to load skills. Reason: cannot find '{0}' file.", propSkillAddPath);
            return;
        }

        using ResourceTableFile propSkill = new(propSkillPath, 1, _defines);
        using var propSkillAdd = new ResourceTableFile(propSkillAddPath, 1, new[] { ',' }, _defines, null);

        var skills = propSkill.GetRecords<SkillProperties>();
        var skillLevelsData = propSkillAdd.GetRecords<SkillLevelProperties>().GroupBy(x => x.SkillId).ToDictionary(x => x.Key, x => x.AsEnumerable());

        foreach (SkillProperties skillProperties in skills)
        {
            if (skillLevelsData.TryGetValue(skillProperties.Id, out IEnumerable<SkillLevelProperties> skillLevels))
            {
                skillProperties.SkillLevels = skillLevels.ToDictionary(x => x.Level, x => x);

                foreach (SkillLevelProperties skillLevel in skillProperties.SkillLevels.Values)
                {
                    if (skillLevel.CooldownTime <= 0)
                    {
                        skillLevel.CooldownTime = skillProperties.SkillReadyTime;
                    }
                }

                if (!_skillsById.TryAdd(skillProperties.Id, skillProperties))
                {
                    _logger.LogWarning($"Cannot add skill '{skillProperties.Name}'. Reason: Already exist.");
                }
            }

        }

        watch.Stop();
        _logger.LogInformation($"{_skillsById.Count} skills loaded in {watch.ElapsedMilliseconds}ms.");
    }
}
