using Microsoft.Extensions.DependencyInjection;
using System;

namespace Rhisis.Core.Extensions;

/// <summary>
/// Provides extensions to the system service provider.
/// </summary>
public static class SerivceProviderExtensions
{
    /// <summary>
    /// Creates a new instance of the given type.
    /// </summary>
    /// <typeparam name="TObject">Object type.</typeparam>
    /// <param name="serviceProvider">Current service provider.</param>
    /// <param name="args">Optional arguments</param>
    /// <returns>Object instance.</returns>
    public static TObject CreateInstance<TObject>(this IServiceProvider serviceProvider, params object[] args) 
        => ActivatorUtilities.CreateInstance<TObject>(serviceProvider, args);
}