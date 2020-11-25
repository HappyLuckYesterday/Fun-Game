using McMaster.Extensions.CommandLineUtils;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using System;
using System.Threading;

namespace Rhisis.CLI.Commands.Database
{
    [Command("update", Description = "Updates the database structure")]
    public class DatabaseUpdateCommand
    {
        private readonly DatabaseFactory _databaseFactory;

        /// <summary>
        /// Gets or sets the database server's host.
        /// </summary>
        [Option(CommandOptionType.SingleValue, ShortName = "s", LongName = "server", Description = "Specify the database host.")]
        public string ServerHost { get; set; }

        /// <summary>
        /// Gets or sets the database server's username.
        /// </summary>
        [Option(CommandOptionType.SingleValue, ShortName = "u", LongName = "user", Description = "Specify the database server user.")]
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the database server username's password.
        /// </summary>
        [Option(CommandOptionType.SingleValue, ShortName = "pwd", LongName = "password", Description = "Specify the database server user's password.")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the database server listening port.
        /// </summary>
        [Option(CommandOptionType.SingleValue, ShortName = "p", LongName = "port", Description = "Specify the database server port.")]
        public int Port { get; set; } = 3306;

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        [Option(CommandOptionType.SingleValue, ShortName = "d", LongName = "database", Description = "Specify the database host.")]
        public string DatabaseName { get; set; }

        /// <summary>
        /// Creates a new <see cref="DatabaseUpdateCommand"/> instance.
        /// </summary>
        /// <param name="databaseFactory">Database factory.</param>
        public DatabaseUpdateCommand(DatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        /// <summary>
        /// Executes the "database update" command.
        /// </summary>
        public void OnExecute()
        {
            try
            {
                var dbConfig = new DatabaseConfiguration
                {
                    Host = ServerHost,
                    Username = User,
                    Password = Password,
                    Port = Port,
                    Database = DatabaseName
                };

                Console.WriteLine("Starting database structure update...");
                TryMigration(dbConfig);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void TryMigration(DatabaseConfiguration databaseConfiguration)
        {
            const int MaxAttempts = 5;
            int attempts = 0;

            while (attempts < MaxAttempts)
            {
                try
                {
                    using IRhisisDatabase database = _databaseFactory.CreateDatabaseInstance(databaseConfiguration);

                    if (database.Exists())
                    {
                        database.Migrate();
                        Console.WriteLine("Database updated.");
                        break;
                    }

                    Console.WriteLine("Database does not exist yet!");
                    break;
                }
                catch
                {
                    Console.WriteLine("Failed to migrate database. New attempt in 5 seconds.");

                    Thread.Sleep(5000);
                    attempts++;
                }
            }
        }
    }
}