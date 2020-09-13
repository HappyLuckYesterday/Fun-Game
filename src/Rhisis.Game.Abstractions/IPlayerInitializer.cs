using Rhisis.Game.Abstractions.Entities;

namespace Rhisis.Game.Abstractions
{
    public interface IPlayerInitializer
    {
        void Load(IPlayer player);

        void Save(IPlayer player);
    }
}
