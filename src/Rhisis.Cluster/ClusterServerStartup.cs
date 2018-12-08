﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Rhisis.Cluster.ISC;
using Rhisis.Core;
using Rhisis.Core.DependencyInjection;
using Rhisis.Core.Helpers;
using Rhisis.Core.Resources;
using Rhisis.Core.Resources.Loaders;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Database;
using Rhisis.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.Cluster
{
    public class ClusterServerStartup : IProgramStartup
    {
        private const string ClusterConfigFile = "config/cluster.json";
        private const string DatabaseConfigFile = "config/database.json";

        private readonly IEnumerable<Type> _loaders = new[]
        {
            typeof(DefineLoader),
            typeof(JobLoader),
        };

        private IClusterServer _server;

        /// <inheritdoc />
        public void Configure()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PacketHandler<ISCClient>.Initialize();
            PacketHandler<ClusterClient>.Initialize();
            DatabaseFactory.Instance.Initialize(DatabaseConfigFile);
            DependencyContainer.Instance.Initialize();
            DependencyContainer.Instance.Register<IClusterServer, ClusterServer>(ServiceLifetime.Singleton);
            DependencyContainer.Instance.Configure(services => services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddNLog(new NLogProviderOptions
                {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
            }));
            DependencyContainer.Instance.Configure(services =>
            {
                var worldConfiguration = ConfigurationHelper.Load<ClusterConfiguration>(ClusterConfigFile, true);
                services.AddSingleton(worldConfiguration);
            });
            GameResources.Instance.Initialize(this._loaders);
            DependencyContainer.Instance.BuildServiceProvider();
        }

        /// <inheritdoc />
        public void Run()
        {
            var logger = DependencyContainer.Instance.Resolve<ILogger<ClusterServerStartup>>();
            this._server = DependencyContainer.Instance.Resolve<IClusterServer>();

            try
            {
                logger.LogInformation("Starting WorldServer...");

                GameResources.Instance.Load();
                this._server.Start();
            }
            catch (Exception e)
            {
                logger.LogCritical(e, $"An unexpected error occured in WorldServer.");
#if DEBUG
                Console.ReadLine();
#endif
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this._server?.Dispose();
            NLog.LogManager.Shutdown();
        }
    }
}