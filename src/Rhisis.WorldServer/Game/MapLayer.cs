namespace Rhisis.WorldServer.Game;

internal class MapLayer
{
    public Map ParentMap { get; }

    public int Id { get; }

    public MapLayer(Map parentMap, int layerId)
    {
        ParentMap = parentMap;
        Id = layerId;
    }
}
