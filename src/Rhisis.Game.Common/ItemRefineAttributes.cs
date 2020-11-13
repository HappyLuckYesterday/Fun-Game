using System.Collections.Generic;

namespace Rhisis.Game.Common
{
    public class ItemRefineAttributes
    {
        /// <summary>
        /// Flyff item refine table.
        /// </summary>
        public static readonly IReadOnlyCollection<int> RefineTable = new[] { 0, 2, 4, 6, 8, 10, 13, 16, 19, 21, 24 };
    }
}
