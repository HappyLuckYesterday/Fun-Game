﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.Structures.Configuration;

namespace Rhisis.Infrastructure.Persistance
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection databaseConfigSection = configuration.GetSection(ConfigurationConstants.DatabaseConfiguration);
            DatabaseOptions databaseConfiguration = databaseConfigSection.Get<DatabaseOptions>();
            var mysqlServerVersion = new MySqlServerVersion(databaseConfiguration.ServerVersion);

            services.AddDbContext<IRhisisDatabase, RhisisDatabaseContext>(options =>
            {
            options.UseMySql(DatabaseFactory.BuildConnectionString(databaseConfiguration), mysqlServerVersion);
            }, ServiceLifetime.Transient);
            services.AddDbContext<RhisisDatabaseContext>(options =>
            {
                options.UseMySql(DatabaseFactory.BuildConnectionString(databaseConfiguration), mysqlServerVersion);
            }, ServiceLifetime.Transient);

            services.AddSingleton(databaseConfiguration); // TODO: remove this and use IOptions<> instead
            services.Configure<DatabaseOptions>(databaseConfigSection);

            return services;
        }
    }
}
