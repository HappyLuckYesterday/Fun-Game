using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Rhisis.Core.Configuration;
using Rhisis.Infrastructure.Persistance.Extensions;
using System;
using System.Data.Common;
using System.IO;
using System.Reflection;

namespace Rhisis.Infrastructure.Persistance.Contexts;

public class BaseDbContext<TContext> : DbContext, IDesignTimeDbContextFactory<TContext> where TContext : DbContext
{
    protected BaseDbContext()
    {
    }

    protected BaseDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public virtual void Migrate()
    {
        if (Database.IsSqlite())
        {
            DbConnectionStringBuilder builder = new()
            {
                ConnectionString = Database.GetConnectionString()
            };

            string filePath = ((string)builder["Data Source"]).Trim();
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Assembly assembly = null;

        if (Database.IsSqlite())
        {
            assembly = Assembly.Load("Rhisis.Infrastructure.Persistance.Sqlite");
        }

        if (assembly is not null)
        {
            modelBuilder.ApplyConfigurationForDbContextFromAssembly<TContext>(assembly);
        }

        base.OnModelCreating(modelBuilder);
    }

    public TContext CreateDbContext(string[] args)
    {
        if (args.Length < 2)
        {
            throw new ArgumentException("Invalid arguments: [provider] [configuration-file]");
        }

        string provider = args[0];
        string configurationFile = args[1];
        string configurationFilePath = Path.Combine(Environment.CurrentDirectory, Path.GetDirectoryName(configurationFile));
        string configurationFileName = Path.GetFileName(configurationFile);

        IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(configurationFilePath)
                .AddYamlFile(configurationFileName)
                .Build();

        DatabaseOptions databaseOptions = configuration.GetSection("database").Get<DatabaseOptions>();

        ArgumentNullException.ThrowIfNull(databaseOptions);
        DbContextOptionsBuilder<TContext> builder = new();

        if (provider == "sqlite")
        {
            builder.UseSqlite(databaseOptions.ConnectionString, options =>
            {
                options.MigrationsAssembly("Rhisis.Infrastructure.Persistance.Sqlite");
            });
        }
        else
        {
            throw new NotImplementedException();
        }

        return Activator.CreateInstance(typeof(TContext), builder.Options) as TContext;
    }

    public override void Dispose()
    {
        Console.WriteLine("Disposed");
        base.Dispose();
    }
}