namespace Rhisis.Network.Core.Servers
{
    /// <summary>
    /// Defines the world channel data structure.
    /// </summary>
    public class WorldChannel : ServerDescriptor
    {
        /// <summary>
        /// Gets the server type.
        /// </summary>
        public override ServerType ServerType => ServerType.World;

        /// <summary>
        /// Gets the parent cluster server id of the current world channel.
        /// </summary>
        public int ClusterId { get; set; }
    }
}
