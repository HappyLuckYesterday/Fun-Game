namespace Rhisis.Game.Abstractions
{
    public interface IArrowProjectile : IProjectile
    {
        /// <summary>
        /// Gets the arrow projectile power.
        /// </summary>
        int Power { get; }
    }
}
