using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Commands.Configure;
using Rhisis.CLI.Commands.Database;
using Rhisis.CLI.Services;
using Rhisis.Database;

namespace Rhisis.CLI.Commands
{
    [Command("setup", Description = "Configure and initialize the database and game servers.")]
    public class SetupCommand
    {
        private readonly DatabaseConfigureCommand _configurationCommand;
        private readonly DatabaseUpdateCommand _updateCommand;
        private readonly LoginServerConfigurationCommand _loginServerConfigurationCommand;
        private readonly ClusterServerConfigurationCommand _clusterServerConfigurationCommand;
        private readonly WorldServerConfigurationCommand _worldServerConfigurationCommand;

        public SetupCommand(DatabaseFactory databaseFactory, ConsoleHelper consoleHelper)
        {
            _configurationCommand = new DatabaseConfigureCommand(consoleHelper);
            _updateCommand = new DatabaseUpdateCommand(databaseFactory);
            _loginServerConfigurationCommand = new LoginServerConfigurationCommand(consoleHelper);
            _clusterServerConfigurationCommand = new ClusterServerConfigurationCommand(consoleHelper);
            _worldServerConfigurationCommand = new WorldServerConfigurationCommand(consoleHelper);
        }

        public void OnExecute()
        {
            _configurationCommand.OnExecute();
            _updateCommand.OnExecute();
            _loginServerConfigurationCommand.OnExecute();
            _clusterServerConfigurationCommand.OnExecute();
            _worldServerConfigurationCommand.OnExecute();
        }
    }
}
