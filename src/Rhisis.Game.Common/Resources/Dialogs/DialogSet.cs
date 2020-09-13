using System.Collections.Generic;

namespace Rhisis.Game.Common.Resources.Dialogs
{
    public class DialogSet : Dictionary<string, DialogData>
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
}