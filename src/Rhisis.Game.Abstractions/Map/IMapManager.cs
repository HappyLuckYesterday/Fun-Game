using Rhisis.Game.Abstractions.Resources;

namespace Rhisis.Game.Abstractions.Map
{
    public interface IMapManager : IGameResourceLoader
    {
        IMap GetMap(int mapId);
    }
}
