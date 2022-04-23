namespace Rhisis.Abstractions.Messaging
{
    /// <summary>
    /// Describes the broadcast message types.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// The cluster message type indicates that the message will be broadcasted to every world channels connected to the cluster.
        /// </summary>
        Cluster,

        /// <summary>
        /// The player message type indicates that the message will be sent to a specific player that is actually connected to one of the cluster channels.
        /// </summary>
        Player
    }
}
