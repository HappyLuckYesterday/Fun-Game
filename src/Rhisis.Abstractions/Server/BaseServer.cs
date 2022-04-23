namespace Rhisis.Abstractions.Server
{
    /// <summary>
    /// Defines the basic server description data structure.
    /// </summary>
    public class BaseServer
    {
        /// <summary>
        /// Gets or sets the server id.
        /// </summary>
        public int Id { get; set; }

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

        /// <summary>
        /// Gets or sets a boolean value that indicates if the server is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;
    }
}
