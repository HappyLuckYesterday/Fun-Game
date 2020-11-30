using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Services;
using Rhisis.Core.Helpers;
using Rhisis.Core.Structures.Configuration;
using System;
using Rhisis.CLI.Core;
using Rhisis.Core.Structures.Configuration.Models;
using Rhisis.Core.Cryptography;

namespace Rhisis.CLI.Commands.Configure
{
    [Command("core", Description = "Configures the Core Server")]
    public class CoreServerConfigurationCommand
    {
        private readonly ConsoleHelper _consoleHelper;

        /// <summary>
        /// Gets or sets the core configuration file to use.
        /// </summary>
        [Option(CommandOptionType.SingleValue, ShortName = "c", LongName = "configuration", Description = "Specify the core server configuration file path.")]
        public string CoreConfigurationFile { get; set; }

        /// <summary>
        /// Gets the real configuration file.
        /// </summary>
        private string ConfigurationFile => !string.IsNullOrEmpty(CoreConfigurationFile) ? CoreConfigurationFile : ConfigurationConstants.CoreServerPath;

        /// <summary>
        /// Creates a new <see cref="CoreServerConfigurationCommand"/> instance.
        /// </summary>
        /// <param name="consoleHelper"></param>
        public CoreServerConfigurationCommand(ConsoleHelper consoleHelper)
        {
            _consoleHelper = consoleHelper;
        }

        /// <summary>
        /// Executes the "configure login" command.
        /// </summary>
        public void OnExecute()
        {
            var coreServerConfiguration = ConfigurationHelper.Load<CoreConfiguration>(ConfigurationFile, ConfigurationConstants.CoreServer);
            var coreConfiguration = new ObjectConfigurationFiller<CoreConfiguration>(coreServerConfiguration);

            Console.WriteLine("----- Core Server -----");
            coreConfiguration.Fill();
            coreConfiguration.Value.Password = MD5.GetMD5Hash(coreConfiguration.Value.Password);

            Console.WriteLine("##### Configuration review #####");
            coreConfiguration.Show("Core server configuration");

            bool response = _consoleHelper.AskConfirmation("Save this configuration?");

            if (!response)
                return;

            ConfigurationHelper.Save(ConfigurationFile, new LoginServerConfigurationModel
            {
                CoreConfiguration = coreConfiguration.Value
            });
            Console.WriteLine($"Core Server configuration saved in {ConfigurationFile}!");
        }
    }
}
