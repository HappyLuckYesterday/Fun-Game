﻿using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Commands.Configure;
using Rhisis.CLI.Commands.Database;
using Rhisis.CLI.Commands.Resources;
using Rhisis.CLI.Commands.User;

namespace Rhisis.CLI
{
    [Command(Description = Description)]
    [Subcommand(typeof(DatabaseCommand))]
    [Subcommand(typeof(UserCommand))]
    [Subcommand(typeof(ConfigureCommand))]
    [Subcommand(typeof(ResourcesCommand))]
    public class Application
    {
        public const string Description = "This tool is a command line interface allowing " +
                                          "administrators to manage their own servers easily.";
        
        public void OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
        }
    }
}
