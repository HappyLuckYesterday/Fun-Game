using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.LoginServer.CoreServer
{
    public class CoreServerService : IHostedService
    {
        private readonly ICoreServer _coreServer;

        public CoreServerService(ICoreServer coreServer)
        {
            _coreServer = coreServer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _coreServer.Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _coreServer.Stop();

            return Task.CompletedTask;
        }
    }
}
