using Rhisis.Game.Common;
using System.Runtime.Serialization;

namespace Rhisis.Game.Resources.Properties;

[DataContract]
public class JobDefinitionProperties
{
    [DataMember]
    public DefineJob.Job? Parent { get; set; }

    [DataMember]
    public DefineJob.JobType Type { get; set; }
}
