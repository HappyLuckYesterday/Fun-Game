using System.Runtime.Serialization;

namespace Rhisis.Game.Common.Resources;

[DataContract]
public class PenalityProperties
{
    [DataMember]
    public int Level { get; set; }

    [DataMember]
    public decimal Value { get; set; }
}
