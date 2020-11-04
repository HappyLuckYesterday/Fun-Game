using System;

namespace Rhisis.Core.Structures
{
    /// <summary>
    /// Provides a simple data structure for storing a range of value.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class Range<TValue> where TValue : struct, IConvertible, IComparable
    {
        /// <summary>
        /// Gets the minimum value of the range.
        /// </summary>
        public TValue Minimum { get; }

        /// <summary>
        /// Gets the maximum value of the range.
        /// </summary>
        public TValue Maximum { get; }

        /// <summary>
        /// Creates a new <see cref="Range{TValue}"/> instance with a given range.
        /// </summary>
        /// <param name="minimum">Lower range bound.</param>
        /// <param name="maximum">Higher range bound.</param>
        public Range(TValue minimum, TValue maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// Checks if the given value is inside the range.
        /// </summary>
        /// <param name="value">Current value.</param>
        /// <returns>True if the current value is inside the range; false otherwise.</returns>
        public bool IsInRange(TValue value)
        {
            return value.CompareTo(Minimum) > 0 && value.CompareTo(Maximum) < 0;
        }
    }
}