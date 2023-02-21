using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Configuration;
using Rhisis.Infrastructure.Persistance.Contexts;
using System;

namespace Rhisis.Infrastructure.Persistance;

/// <summary>
/// Provides extensions to initialize the persistance layer.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the acccount persistance mechanism to the current service collection.
    /// </summary>
    /// <param name="services">Current service collection instance.</param>
    /// <param name="databaseOptions">Database options.</param>
    /// <returns>Current service collection instance.</returns>
    public static IServiceCollection AddAccountPersistance(this IServiceCollection services, DatabaseOptions databaseOptions)
    {
        return AddPersistance<IAccountDatabase, AccountDbContext>(services, databaseOptions);
    }

    /// <summary>
    /// Adds the game persistance mechanism to the current service collection.
    /// </summary>
    /// <param name="services">Current service collection instance.</param>
    /// <param name="databaseOptions">Database options.</param>
    /// <returns>Current service collection instance.</returns>
    public static IServiceCollection AddGamePersistance(this IServiceCollection services, DatabaseOptions databaseOptions)
    {
        return AddPersistance<IGameDatabase, GameDbContext>(services, databaseOptions);
    }

    /// <summary>
    /// Adds the persistance layer based on the generic parameters and given database options.
    /// </summary>
    /// <typeparam name="TContext">Database context abstraction.</typeparam>
    /// <typeparam name="TContextImplementation">Database context implementation.</typeparam>
    /// <param name="services">Current service collection instance.</param>
    /// <param name="databaseOptions">Database options.</param>
    /// <returns>Current service collection instance.</returns>
    /// <exception cref="NotImplementedException">Thrown when a persistance provider is not implemented.</exception>
    private static IServiceCollection AddPersistance<TContext, TContextImplementation>(this IServiceCollection services, DatabaseOptions databaseOptions)
        where TContextImplementation : DbContext, TContext
    {
        ArgumentNullException.ThrowIfNull(databaseOptions);
        ArgumentException.ThrowIfNullOrEmpty(databaseOptions.ConnectionString);

        services.AddDbContext<TContext, TContextImplementation>(options =>
        {
            if (databaseOptions.Provider is DatabaseProviders.Sqlite)
            {
                options.UseSqlite(databaseOptions.ConnectionString, builder =>
                {
                    builder.MigrationsAssembly("Rhisis.Infrastructure.Persistance.Sqlite");
                });
            }
            else
            {
                throw new NotImplementedException($"Provider '{databaseOptions.Provider}' is not implemented.");
            }
        }, ServiceLifetime.Transient);

        return services;
    }
}
