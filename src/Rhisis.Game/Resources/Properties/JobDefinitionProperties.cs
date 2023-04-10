using System.Runtime.Serialization;

namespace Rhisis.Game.Common.Resources;

[DataContract]
public class JobDefinitionProperties
{
    [DataMember]
    public DefineJob.Job? Parent { get; set; }

    [DataMember]
    public DefineJob.JobType Type { get; set;}
}
