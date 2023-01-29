using System;
using System.Linq;
using System.Reflection;

namespace Rhisis.Core.Extensions;

public static class ReflectionExtensions
{
    /// <summary>
    /// Check if the given source type implements the given interface type.
    /// </summary>
    /// <param name="sourceType">Source type.</param>
    /// <param name="interfaceType">Interface type.</param>
    /// <returns>True if the source type implements the given generic type.</returns>
    /// <exception cref="ArgumentNullException">Source type or given interface type is null.</exception>
    /// <exception cref="ArgumentException">Given interface type is not an interface.</exception>
    public static bool ImplementsInterface(this Type sourceType, Type interfaceType)
    {
        if (sourceType is null)
        {
            throw new ArgumentNullException(nameof(sourceType));
        }

        if (interfaceType is null)
        {
            throw new ArgumentNullException(nameof(interfaceType));
        }

        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException($"The given interface type '{interfaceType.FullName}' is not an interface.");
        }

        if (interfaceType.IsGenericType)
        {
            return sourceType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == interfaceType);
        }
        else
        {
            return sourceType.GetInterfaces().Any(x => x == interfaceType);
        }
    }

    /// <summary>
    /// Assign the given value to the object's property.
    /// </summary>
    /// <param name="object">Current object.</param>
    /// <param name="propertyName">Object property name.</param>
    /// <param name="value">Value.</param>
    public static void AssignProperty(this object @object, string propertyName, object value)
    {
        PropertyInfo property = @object.GetType().GetProperty(propertyName);

        property?.SetValue(@object, value, null);
    }
}
