using Rhisis.Game.Common;
using System.Runtime.Serialization;

namespace Rhisis.Game.Common.Resources
{
    [DataContract]
    public class JobDefinitionData
    {
        [DataMember]
        public DefineJob.Job? Parent { get; set; }

        [DataMember]
        public DefineJob.JobType Type { get; set;}
    }
}
