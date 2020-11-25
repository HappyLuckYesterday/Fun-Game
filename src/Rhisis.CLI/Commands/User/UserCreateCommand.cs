using McMaster.Extensions.CommandLineUtils;
using Rhisis.CLI.Services;
using Rhisis.Core.Cryptography;
using Rhisis.Core.Extensions;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Database.Entities;
using Rhisis.Game.Common;
using System;
using System.Linq;

namespace Rhisis.CLI.Commands.User
{
    [Command("create", Description = "Create a new user")]
    public sealed class UserCreateCommand
    {
        private readonly DatabaseFactory _databaseFactory;
        private readonly ConsoleHelper _consoleHelper;

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

        public UserCreateCommand(DatabaseFactory databaseFactory, ConsoleHelper consoleHelper)
        {
            _databaseFactory = databaseFactory;
            _consoleHelper = consoleHelper;
        }

        public void OnExecute()
        {
            var dbConfig = new DatabaseConfiguration
            {
                Host = ServerHost,
                Username = User,
                Password = Password,
                Port = Port,
                Database = DatabaseName
            };

            var user = new DbUser();

            Console.Write("Username: ");
            user.Username = Console.ReadLine();

            Console.Write("Email: ");
            user.Email = Console.ReadLine();

            Console.Write("Password: ");
            user.Password = _consoleHelper.ReadPassword();

            Console.Write("Password confirmation: ");
            string passwordConfirmation = _consoleHelper.ReadPassword();

            Console.Write("Password salt (kikugalanet): ");
            string passwordSalt = _consoleHelper.ReadStringOrDefault("kikugalanet");

            Console.WriteLine("Authority: ");
            _consoleHelper.DisplayEnum<AuthorityType>();
            user.Authority = (int)_consoleHelper.ReadEnum<AuthorityType>();

            Console.WriteLine("--------------------------------");
            Console.WriteLine("User account information:");
            Console.WriteLine($"Username: {user.Username}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Authority: {(AuthorityType)user.Authority}");
            Console.WriteLine("--------------------------------");

            bool response = _consoleHelper.AskConfirmation("Create user?");

            if (response)
            {
                using IRhisisDatabase database = _databaseFactory.CreateDatabaseInstance(dbConfig);
                
                if (database.Users.Any(x => x.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"User '{user.Username}' is already used.");
                    return;
                }

                if (!user.Email.IsValidEmail())
                {
                    Console.WriteLine($"Email '{user.Email}' is not valid.");
                    return;
                }

                if (database.Users.Any(x => x.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine($"Email '{user.Email}' is already used.");
                    return;
                }

                if (!user.Password.Equals(passwordConfirmation))
                {
                    Console.WriteLine("Passwords doesn't match.");
                    return;
                }

                user.Password = MD5.GetMD5Hash(passwordSalt, user.Password);

                database.Users.Add(user);
                database.SaveChanges();

                Console.WriteLine($"User '{user.Username}' created.");
            }
        }
    }
}
