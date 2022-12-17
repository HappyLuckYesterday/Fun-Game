using System;
using System.Text.Json.Serialization;

namespace Rhisis.Protocol.Messages;

public class CoreMessage
{
    /// <summary>
    /// Gets or sets the core message type.
    /// </summary>
    [JsonIgnore]
    public Type Type { get; set; }
}
