using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to manage a world entity attributes.
    /// </summary>
    public interface IAttributes
    {
        /// <summary>
        /// Gets the value associated to the given attribute.
        /// </summary>
        /// <param name="attribute">Attribute.</param>
        /// <param name="defaultValue">Default attribute value in case the attribute is not set.</param>
        /// <returns>Attribute value; or if not exists, default value.</returns>
        int Get(DefineAttributes attribute, int defaultValue = 0);

        /// <summary>
        /// Sets the attribute to the given value.
        /// </summary>
        /// <param name="attribute">Attribute to set.</param>
        /// <param name="value">Value to give to the attribute.</param>
        /// <param name="sendToEntity">When true, sends the attribute update to the player.</param>
        void Set(DefineAttributes attribute, int value, bool sendToEntity = true);

        /// <summary>
        /// Increases the attribute by the given value.
        /// </summary>
        /// <param name="attribute">Attribute to increase.</param>
        /// <param name="value">Value to increase for the attribute.</param>
        /// <param name="sendToEntity">When true, sends the attribute update to the player.</param>
        void Increase(DefineAttributes attribute, int value, bool sendToEntity = true);

        /// <summary>
        /// Decreases the attribute by the given value.
        /// </summary>
        /// <param name="attribute">Attribute to decrease.</param>
        /// <param name="value">Value to decrease for the attribute.</param>
        /// <param name="sendToEntity">When true, sends the attribute update to the player.</param>
        void Decrease(DefineAttributes attribute, int value, bool sendToEntity = true);
    }
}
