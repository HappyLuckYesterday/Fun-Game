﻿using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Core;
using Rhisis.CLI.Services;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Core.Structures.Configuration.World;
using System;
using System.Collections.Generic;

namespace Rhisis.CLI.Commands.Configure
{
    [Command("world", Description = "Configures the World Server")]
    public sealed class WorldServerConfigurationCommand
    {
        private readonly ConsoleHelper _consoleHelper;

        /// <summary>
        /// Gets or sets the cluster configuration file to use.
        /// </summary>
        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration", Description = "Specify the world server configuration file path.")]
        public string WorldConfigurationFile { get; set; }

        /// <summary>
        /// Gets the real configuration file.
        /// </summary>
        private string ConfigurationFile => !string.IsNullOrEmpty(this.WorldConfigurationFile) ? this.WorldConfigurationFile : ConfigurationConstants.WorldServerPath;


        /// <summary>
        /// Creates a new <see cref="WorldServerConfigurationCommand"/> instance.
        /// </summary>
        /// <param name="consoleHelper">Console helpers.</param>
        public WorldServerConfigurationCommand(ConsoleHelper consoleHelper)
        {
            this._consoleHelper = consoleHelper;
        }

        /// <summary>
        /// Executes the "configure world" command.
        /// </summary>
        public void OnExecute()
        {
            var worldServerConfiguration = ConfigurationHelper.Load<WorldConfiguration>(this.ConfigurationFile, ConfigurationConstants.WorldServer);
            var coreServerConfiguratinon = ConfigurationHelper.Load<CoreConfiguration>(this.ConfigurationFile, ConfigurationConstants.CoreServer);
            var worldConfiguration = new ObjectConfigurationFiller<WorldConfiguration>(worldServerConfiguration);
            var coreConfiguration = new ObjectConfigurationFiller<CoreConfiguration>(coreServerConfiguratinon);

            Console.WriteLine("----- World Server -----");
            worldConfiguration.Fill();
            Console.WriteLine("----- Core Server -----");
            coreConfiguration.Fill();

            Console.WriteLine("##### Configuration review #####");
            worldConfiguration.Show("World Server configuration");
            coreConfiguration.Show("Core server configuration");

            bool response = this._consoleHelper.AskConfirmation("Save this configuration?");

            if (response)
            {
                var configuration = new Dictionary<string, object>
                {
                    { ConfigurationConstants.WorldServer, worldConfiguration.Value },
                    { ConfigurationConstants.CoreServer, coreConfiguration.Value }
                };

                ConfigurationHelper.Save(this.ConfigurationFile, configuration);
                Console.WriteLine($"World Server configuration saved in {this.ConfigurationFile}!");
            }
        }
    }
}
