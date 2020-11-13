namespace Rhisis.Core
{
    public interface ICloneable<T>
    {
        /// <summary>
        /// Clones the current object into a new instance.
        /// </summary>
        /// <returns></returns>
        T Clone();
    }
}
