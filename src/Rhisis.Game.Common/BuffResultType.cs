namespace Rhisis.Game.Common
{
    /// <summary>
    /// Provides an enumeration that describes all possible states of a buff when casted.
    /// </summary>
    public enum BuffResultType
    {
        /// <summary>
        /// Nothing has been done.
        /// </summary>
        None,

        /// <summary>
        /// The buff has been added to the mover.
        /// </summary>
        Added,

        /// <summary>
        /// The buff has been updated.
        /// </summary>
        Updated,

        /// <summary>
        /// The buff has been removed.
        /// </summary>
        Removed
    }
}