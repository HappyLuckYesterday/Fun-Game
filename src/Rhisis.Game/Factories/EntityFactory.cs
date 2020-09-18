using Microsoft.Extensions.DependencyInjection;
using Rhisis.Core.DependencyInjection;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Factories;
using Rhisis.Game.Entities;

namespace Rhisis.Game.Factories
{
    [Injectable(ServiceLifetime.Singleton)]
    internal class EntityFactory : IEntityFactory
    {
        public IMapItem CreateMapItem()
        {
            return null;
        }

        public IMonster CreateMonster()
        {
            return null;
        }

        public INpc CreateNpc()
        {
            return null;
        }
    }
}
