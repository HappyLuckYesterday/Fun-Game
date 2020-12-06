using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Core;
using Rhisis.CLI.Services;
using Rhisis.Core.Cryptography;
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
        private string ConfigurationFile => !string.IsNullOrEmpty(WorldConfigurationFile) ? WorldConfigurationFile : ConfigurationConstants.WorldServerPath;


        /// <summary>
        /// Creates a new <see cref="WorldServerConfigurationCommand"/> instance.
        /// </summary>
        /// <param name="consoleHelper">Console helpers.</param>
        public WorldServerConfigurationCommand(ConsoleHelper consoleHelper)
        {
            _consoleHelper = consoleHelper;
        }

        /// <summary>
        /// Executes the "configure world" command.
        /// </summary>
        public void OnExecute()
        {
            var worldServerConfiguration = ConfigurationHelper.Load<WorldConfiguration>(ConfigurationFile, ConfigurationConstants.WorldServer);
            var coreConfiguration = ConfigurationHelper.Load<CoreConfiguration>(ConfigurationFile, ConfigurationConstants.CoreServer);
            var worldConfiguration = new ObjectConfigurationFiller<WorldConfiguration>(worldServerConfiguration);
            var coreServerConfiguration = new ObjectConfigurationFiller<CoreConfiguration>(coreConfiguration);

            Console.WriteLine("----- Core Server -----");
            coreServerConfiguration.Fill();
            coreServerConfiguration.Value.Password = MD5.GetMD5Hash(coreServerConfiguration.Value.Password);

            Console.WriteLine("----- World Server -----");
            worldConfiguration.Fill();
            worldConfiguration.Value.Maps = new List<string>
            {
                "WI_WORLD_MADRIGAL",
                "WI_DUNGEON_FL_MAS"
            };

            Console.WriteLine("##### Configuration review #####");
            worldConfiguration.Show("World Server configuration");
            coreServerConfiguration.Show("Core Server configuration");

            bool response = _consoleHelper.AskConfirmation("Save this configuration?");

            if (response)
            {
                var configuration = new Dictionary<string, object>
                {
                    { ConfigurationConstants.WorldServer, worldConfiguration.Value },
                    { ConfigurationConstants.CoreServer, coreServerConfiguration.Value }
                };

                ConfigurationHelper.Save(ConfigurationFile, configuration);
                Console.WriteLine($"World Server configuration saved in {ConfigurationFile}!");
            }
        }
    }
}
