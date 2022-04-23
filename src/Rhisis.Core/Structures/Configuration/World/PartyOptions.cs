using System.Runtime.Serialization;

namespace Rhisis.Core.Structures.Configuration.World
{
    [DataContract]
    public class PartyOptions
    {
        public const int DefaultMaxPartyMemberCount = 8;

        /// <summary>
        /// Gets or sets the maximum amount of members in a party.
        /// </summary>
        public int MaxPartyMemberCount { get; set; }
        
        /// <summary>
        /// Creates a new <see cref="PartyOptions"/> instance.
        /// </summary>
        public PartyOptions()
        {
            MaxPartyMemberCount = DefaultMaxPartyMemberCount;
        }
    }
}
