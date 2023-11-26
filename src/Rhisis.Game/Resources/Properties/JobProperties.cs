﻿using Rhisis.Game.Common;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Rhisis.Game.Resources.Properties;

[DataContract]
[DebuggerDisplay("{Id}")]
public class JobProperties
{
    /// <summary>
    /// Gets or sets the job Id.
    /// </summary>
    [DataMember(Order = 0)]
    public DefineJob.Job Id { get; set; }

    [DataMember(Order = 1)]
    public float AttackSpeed { get; set; }

    [DataMember(Order = 2)]
    public float MaxHpFactor { get; set; }

    [DataMember(Order = 3)]
    public float MaxMpFactor { get; set; }

    [DataMember(Order = 4)]
    public float MaxFpFactor { get; set; }

    [DataMember(Order = 5)]
    public float DefenseFactor { get; set; }

    [DataMember(Order = 6)]
    public float HpRecoveryFactor { get; set; }

    [DataMember(Order = 7)]
    public float MpRecoveryFactor { get; set; }

    [DataMember(Order = 8)]
    public float FpRecoveryFactor { get; set; }

    [DataMember(Order = 9)]
    public float MeleeSword { get; set; }

    [DataMember(Order = 10)]
    public float MeleeAxe { get; set; }

    [DataMember(Order = 11)]
    public float MeleeStaff { get; set; }

    [DataMember(Order = 12)]
    public float MeleeStick { get; set; }

    [DataMember(Order = 13)]
    public float MeleeKnuckle { get; set; }

    [DataMember(Order = 14)]
    public float MagicWand { get; set; }

    [DataMember(Order = 15)]
    public float Blocking { get; set; }

    [DataMember(Order = 16)]
    public float MeleeYoyo { get; set; }

    [DataMember(Order = 17)]
    public float Critical { get; set; }

    [IgnoreDataMember]
    public DefineJob.JobType Type { get; set; }

    [IgnoreDataMember]
    public JobProperties Parent { get; set; }

    [IgnoreDataMember]
    public int MinLevel
    {
        get => Type switch
        {
            DefineJob.JobType.JTYPE_BASE => 1,
            DefineJob.JobType.JTYPE_EXPERT => (int)DefineJob.JobMax.MAX_JOB_LEVEL,
            DefineJob.JobType.JTYPE_PRO => (int)DefineJob.JobMax.MAX_JOB_LEVEL + (int)DefineJob.JobMax.MAX_EXP_LEVEL,
            _ => (int)DefineJob.JobMax.MAX_LEVEL
        };
    }

    [IgnoreDataMember]
    public int MaxLevel
    {
        get => Type switch
        {
            DefineJob.JobType.JTYPE_BASE => (int)DefineJob.JobMax.MAX_JOB_LEVEL,
            DefineJob.JobType.JTYPE_EXPERT => (int)DefineJob.JobMax.MAX_JOB_LEVEL + (int)DefineJob.JobMax.MAX_EXP_LEVEL,
            DefineJob.JobType.JTYPE_PRO => (int)DefineJob.JobMax.MAX_LEVEL,
            _ => (int)DefineJob.JobMax.MAX_LEVEL
        };
    }

    /// <summary>
    /// Checks if the given job is anterior to the player's job.
    /// </summary>
    /// <param name="player">Current player.</param>
    /// <param name="job">Job.</param>
    /// <returns>True if the given job is anterior to the player's job; false otherwise.</returns>
    public bool IsAnteriorJob(DefineJob.Job job)
    {
        JobProperties jobData = this;

        while (jobData != null)
        {
            if (jobData.Id == job)
            {
                return true;
            }

            jobData = jobData.Parent;
        }

        return false;
    }
}
