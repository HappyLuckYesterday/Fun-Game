using System;

namespace Rhisis.Game.Abstractions.Behavior
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class BehaviorAttribute : Attribute
    {
        public const int DefaultMoverId = 0;

        /// <summary>
        /// Gets the mover's id.
        /// </summary>
        public int MoverId { get; }

        /// <summary>
        /// Get the behavior type.
        /// </summary>
        public BehaviorType Type { get; }

        /// <summary>
        /// Gets the default state of the behavior.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Creates a new default <see cref="BehaviorAttribute"/> instance.
        /// </summary>
        /// <param name="behaviorType">Behavior type</param>
        public BehaviorAttribute(BehaviorType behaviorType)
            : this(behaviorType, DefaultMoverId)
        { }

        /// <summary>
        /// Creates a new <see cref="BehaviorAttribute"/> for a specific mover id.
        /// </summary>
        /// <param name="behaviorType">Behavior type</param>
        /// <param name="moverId"></param>
        public BehaviorAttribute(BehaviorType behaviorType, int moverId)
        {
            Type = behaviorType;
            MoverId = moverId;
            IsDefault = false;
        }
    }
}
