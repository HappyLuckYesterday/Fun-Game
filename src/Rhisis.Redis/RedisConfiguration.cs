namespace Rhisis.Redis
{
    public class RedisConfiguration
    {
        /// <summary>
        /// Gets or sets the redis host.
        /// </summary>
        public string Host { get; set; } = "127.0.0.1";

        /// <summary>
        /// Gets or sets the redis port.
        /// </summary>
        /// <remarks>
        /// Default value is 6379.
        /// </remarks>
        public ushort Port { get; set; } = 6379;
    }
}
