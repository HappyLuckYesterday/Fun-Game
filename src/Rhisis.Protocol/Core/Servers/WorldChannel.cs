namespace Rhisis.Protocol.Core.Servers
{
    /// <summary>
    /// Defines the world channel data structure.
    /// </summary>
    public class WorldChannel : BaseServer
    {
        /// <summary>
        /// Gets the server type.
        /// </summary>
        public override ServerType ServerType => ServerType.World;

        /// <summary>
        /// Gets the parent cluster server id of the current world channel.
        /// </summary>
        public int ClusterId { get; set; }

        /// <summary>
        /// Gets or sets the number of maximum connected users allowed.
        /// </summary>
        public int MaximumUsers { get; set; }

        /// <summary>
        /// Gets or sets the number of connected users on the current channel.
        /// </summary>
        public int ConnectedUsers { get; set; }
    }
}
