using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration.World
{
    /// <summary>
    /// Represents the perin configuration section.
    /// </summary>
    public class PerinOptions
    {
        public const int DefaultPerinValue = 100000000;

        /// <summary>
        /// Gets or sets perin value.
        /// </summary>
        [DataMember(Name = "perinValue")]
        public int PerinValue { get; set; }

        /// <summary>
        /// Creates a new <see cref="PerinOptions"/> instance.
        /// </summary>
        public PerinOptions()
        {
            PerinValue = DefaultPerinValue;
        }
    }
}
