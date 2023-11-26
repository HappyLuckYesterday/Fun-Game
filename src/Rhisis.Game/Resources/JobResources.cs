using Microsoft.Extensions.Logging;
using Rhisis.Game.Common;
using Rhisis.Game.IO;
using Rhisis.Game.Resources.Properties;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rhisis.Game.Resources;

public sealed class JobResources
{
    private readonly ILogger<JobResources> _logger;
    private readonly ConcurrentDictionary<string, int> _defines;
    private readonly ConcurrentDictionary<DefineJob.Job, JobProperties> _jobsById = new();

    public JobResources(ILogger<JobResources> logger, ConcurrentDictionary<string, int> defines)
    {
        _logger = logger;
        _defines = defines;
    }

    public JobProperties Get(DefineJob.Job job) => _jobsById.TryGetValue(job, out JobProperties jobProperties) ? jobProperties : null;

    public JobProperties Get(int jobId) => Get((DefineJob.Job)jobId);

    public JobProperties Find(Func<JobProperties, bool> predicate) => _jobsById.Values.FirstOrDefault(predicate);

    public void Load()
    {
        Stopwatch watch = new();
        watch.Start();
        string propJobFile = GameResourcePaths.JobPropPath;
        string jobsDefinitionFile = GameResourcePaths.JobsDefinitionsPath;

        if (!File.Exists(propJobFile))
        {
            _logger.LogWarning($"Unable to load job properties. Reason: cannot find '{propJobFile}' file.");
            return;
        }

        if (!File.Exists(jobsDefinitionFile))
        {
            _logger.LogWarning($"Unable to load job definitions. Reason: cannot find '{jobsDefinitionFile}' file.");
            return;
        }

        string jobDefinitionFileContent = File.ReadAllText(jobsDefinitionFile);
        var jobDefinitions = JsonSerializer.Deserialize<Dictionary<DefineJob.Job, JobDefinitionProperties>>(jobDefinitionFileContent, new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            }
        });

        using (var propJob = new ResourceTableFile(propJobFile, -1, new[] { '\t', ' ', '\r' }, _defines, null))
        {
            IEnumerable<JobProperties> jobs = propJob.GetRecords<JobProperties>();

            foreach (JobProperties job in jobs)
            {
                if (!_jobsById.TryAdd(job.Id, job))
                {
                    _logger.LogWarning($"Cannot add job properties '{job.Id}'. Reason: Already exist.");
                }
            }

            foreach (JobProperties job in _jobsById.Values)
            {
                if (jobDefinitions.TryGetValue(job.Id, out JobDefinitionProperties jobDefinition))
                {
                    job.Parent = jobDefinition.Parent.HasValue ? _jobsById[jobDefinition.Parent.Value] : null;
                    job.Type = jobDefinition.Type;
                }
                else
                {
                    _logger.LogWarning($"Cannot find job '{job.Id}' definition.");
                }
            }
        }

        watch.Stop();
        _logger.LogInformation($"{_jobsById.Count} skills loaded in {watch.ElapsedMilliseconds}ms.");
    }
}
