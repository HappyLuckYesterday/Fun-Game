namespace Rhisis.Messaging.RabbitMQ
{
    public class RabbitMQBuilderOptions
    {
        /// <summary>
        /// Gets or sets the RabbitMQ host.
        /// </summary>
        public string Host { get; set; } = "localhost";

        /// <summary>
        /// Gets or sets the RabbitMQ username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the RabbitMQ password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Internally creates a new <see cref="RabbitMQBuilderOptions"/> instance.
        /// </summary>
        internal RabbitMQBuilderOptions()
        {
        }
    }
}
