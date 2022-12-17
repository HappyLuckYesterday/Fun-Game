using Rhisis.Game.Common;
using System.Text.Json.Serialization;

namespace Rhisis.Protocol.Messages.Cluster;

public class PlayerConnectedMessage : CoreMessage
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("status")]
    public MessengerStatusType Status { get; set; }
}
