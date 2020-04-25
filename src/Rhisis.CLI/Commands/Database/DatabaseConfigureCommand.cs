﻿using System;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using Rhisis.CLI.Core;
using Rhisis.CLI.Services;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Structures.Configuration.Models;
using Rhisis.Database;

namespace Rhisis.CLI.Commands.Database
{
    [Command("configure", Description = "Configures the database access")]
    public class DatabaseConfigureCommand
    {
        /// <summary>
        /// Gets or sets the database configuration file.
        /// </summary>
        /// <remarks>
        /// If the database configuration file is not specified, the <see cref="ConfigurationConstants.DatabasePath"/> constant is used instead.
        /// </remarks>
        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration", Description = "Specify the database configuration file path.")]
        public string DatabaseConfigurationFile { get; set; }

        private readonly ConsoleHelper _consoleHelper;

        /// <summary>
        /// Creates a new <see cref="DatabaseConfigureCommand"/> instance.
        /// </summary>
        /// <param name="consoleHelper">Console helper.</param>
        public DatabaseConfigureCommand(ConsoleHelper consoleHelper)
        {
            _consoleHelper = consoleHelper;
        }

        /// <summary>
        /// Executes the "database configure" command.
        /// </summary>
        public void OnExecute()
        {
            if (string.IsNullOrEmpty(DatabaseConfigurationFile))
                DatabaseConfigurationFile = ConfigurationConstants.DatabasePath;

            var databaseConfiguration = ConfigurationHelper.Load<DatabaseConfiguration>(DatabaseConfigurationFile, ConfigurationConstants.DatabaseConfiguration);
            var dbConfiguration = new ObjectConfigurationFiller<DatabaseConfiguration>(databaseConfiguration);

            dbConfiguration.Fill();

            if (dbConfiguration.Value.UseEncryption)
            {
                if (string.IsNullOrEmpty(dbConfiguration.Value.EncryptionKey))
                {
                    dbConfiguration.Value.EncryptionKey = Convert.ToBase64String(AesProvider.GenerateKey(AesKeySize.AES256Bits).Key);
                }
                else
                {
                    Console.WriteLine("Warning: your database configuration already contains an encryption key.");
                }
            }

            dbConfiguration.Show("Database configuration");
            Console.WriteLine($"Encryption key: {dbConfiguration.Value.EncryptionKey}");

            bool response = _consoleHelper.AskConfirmation("Save this configuration?");

            if (response)
            {
                ConfigurationHelper.Save(DatabaseConfigurationFile, new DatabaseConfigurationModel()
                {
                    DatabaseConfiguration = dbConfiguration.Value
                });
                Console.WriteLine($"Database configuration saved in '{DatabaseConfigurationFile}'.");
            }
        }
    }
}
