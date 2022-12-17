using Rhisis.Abstractions.Map;

namespace Rhisis.Game.Map;

public class MapNpcObject : MapObject, IMapNpcObject
{
    public string Name { get; set; }
}
