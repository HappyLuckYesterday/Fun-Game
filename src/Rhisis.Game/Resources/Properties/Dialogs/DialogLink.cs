using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Rhisis.Game.Resources.Properties.Dialogs;

public class DialogLink
{
    /// <summary>
    /// Gets or sets the dialog link id.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the dialog link title.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the dialog link texts.
    /// </summary>
    /// <remarks>
    /// This text will appear once the client has clicked on the link title.
    /// </remarks>
    [JsonPropertyName("texts")]
    public IList<string> Texts { get; set; }

    /// <summary>
    /// Gets or sets the link quest id.
    /// </summary>
    [JsonIgnore]
    public int? QuestId { get; set; }
}