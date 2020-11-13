namespace Rhisis.Game.Abstractions.Map
{
    public interface IMapNpcObject : IMapObject
    {
        /// <summary>
        /// Gets the map NPC name.
        /// </summary>
        string Name { get; }
    }
}
