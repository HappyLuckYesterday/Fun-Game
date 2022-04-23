namespace Rhisis.Core.Structures.Configuration
{
    /// <summary>
    /// Represents the cluster cache options.
    /// </summary>
    public class ClusterCacheOptions
    {
        /// <summary>
        /// Gets or sets the cluster cache host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the cluster cache listening port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets ro sets the cluster cache password.
        /// </summary>
        public string Password { get; set; }
    }
}
