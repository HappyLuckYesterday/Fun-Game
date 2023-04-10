using System.Runtime.Serialization;

namespace Rhisis.Game.Resources.Properties;

[DataContract]
public class DropItemProperties
{
    [DataMember]
    public int ItemId { get; set; }

    [DataMember]
    public long Probability { get; set; }

    [DataMember]
    public int ItemMaxRefine { get; set; }

    [DataMember]
    public int Count { get; set; }
}
