using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rhisis.World.Systems.Buff
{
    public interface IBuffSystem
    {
    }

    [Injectable(ServiceLifetime.Singleton)]
    public class BuffSystem : IBuffSystem
    {
    }
}
