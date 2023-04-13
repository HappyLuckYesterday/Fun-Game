using System.Collections.Generic;

namespace Rhisis.Game.Resources.Properties.Dialogs;

public class DialogSet : Dictionary<string, DialogProperties>
{
    /// <summary>
    /// Gets the DialogSet's language.
    /// </summary>
    public string Language { get; }

    /// <summary>
    /// Creates a new <see cref="DialogSet"/> instance.
    /// </summary>
    /// <param name="language"></param>
    public DialogSet(string language)
    {
        Language = language;
    }
}