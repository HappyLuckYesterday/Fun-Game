namespace Rhisis.Network.Core.Servers
{
    /// <summary>
    /// Defines the basic server description data structure.
    /// </summary>
    public abstract class ServerDescriptor
    {
        /// <summary>
        /// Gets or sets the server id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets the server type.
        /// </summary>
        public abstract ServerType ServerType { get; }

        /// <summary>
        /// Gets or sets the server host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the server listening port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the server's name.
        /// </summary>
        public string Name { get; set; }
    }
}
