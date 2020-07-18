using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;

namespace Rhisis.World.Systems.Buff
{
    [Injectable(ServiceLifetime.Singleton)]
    public class BuffSystem : IBuffSystem
    {
    }
}
