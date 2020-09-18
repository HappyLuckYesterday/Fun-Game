using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Entities;

namespace Rhisis.Game.Abstractions.Factories
{
    public interface IEntityFactory
    {
        IMonster CreateMonster();

        INpc CreateNpc();

        IMapItem CreateMapItem();
    }
}
