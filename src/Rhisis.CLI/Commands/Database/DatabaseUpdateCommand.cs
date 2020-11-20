using System;
using System.Threading;
using McMaster.Extensions.CommandLineUtils;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;

namespace Rhisis.CLI.Commands.Database
{
    [Command("update", Description = "Updates the database structure")]
    public class DatabaseUpdateCommand
    {
        private readonly DatabaseFactory _databaseFactory;

        /// <summary>
        /// Gets or sets the database configuration file.
        /// </summary>
        /// <remarks>
        /// If the database configuration file is not specified, the <see cref="ConfigurationConstants.DatabasePath"/> constant is used instead.
        /// </remarks>
        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration", Description = "Specify the database configuration file path.")]
        public string DatabaseConfigurationFile { get; set; }

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
                if (string.IsNullOrEmpty(DatabaseConfigurationFile))
                {
                    DatabaseConfigurationFile = ConfigurationConstants.DatabasePath;
                }

                DatabaseConfiguration dbConfig = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigurationFile, ConfigurationConstants.DatabaseConfiguration);
                
                if (dbConfig is null)
                {
                    Console.WriteLine("Couldn't load database configuration file during execution of update command.");
                    return;
                }

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