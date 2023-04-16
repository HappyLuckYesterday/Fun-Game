using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Rhisis.Game.Resources.Properties.Dialogs;

[DataContract]
public class DialogProperties
{
    /// <summary>
    /// Gets or sets the dialog name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the dialog's oral text.
    /// </summary>
    [JsonPropertyName("oralText")]
    public string OralText { get; set; }

    /// <summary>
    /// Gets or sets the dialog's introduction text.
    /// </summary>
    [JsonPropertyName("introText")]
    public IEnumerable<string> IntroText { get; set; }

    /// <summary>
    /// Gets or sets the dialog's goodbye text.
    /// </summary>
    [JsonPropertyName("byeText")]
    public string ByeText { get; set; }

    /// <summary>
    /// Gets the dialog's links.
    /// </summary>
    [JsonPropertyName("links")]
    public List<DialogLink> Links { get; set; }

    /// <summary>
    /// Creates a new <see cref="DialogProperties"/> instance.
    /// </summary>
    public DialogProperties()
        : this(string.Empty, string.Empty, string.Empty, string.Empty)
    {
    }

    /// <summary>
    /// Creates a new <see cref="DialogProperties"/> instance.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="oralText"></param>
    /// <param name="introText"></param>
    /// <param name="byeText"></param>
    public DialogProperties(string name, string oralText, string introText, string byeText)
    {
        Name = name;
        OralText = oralText;
        IntroText = new[] { introText };
        ByeText = byeText;
        Links = new List<DialogLink>();
    }
}