using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions.Factories
{
    public interface IEntityFactory
    {
        IMonster CreateMonster();

        INpc CreateNpc();
    }
}
