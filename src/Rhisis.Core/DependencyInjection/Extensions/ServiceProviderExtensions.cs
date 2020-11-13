using Microsoft.Extensions.DependencyInjection;
using System;

namespace Rhisis.Core.DependencyInjection.Extensions
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Creates a new instance of <typeparamref name="TInstance"/> using the service provider to inject dependencies.
        /// </summary>
        /// <typeparam name="TInstance"></typeparam>
        /// <param name="serviceProvider"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static TInstance CreateInstance<TInstance>(this IServiceProvider serviceProvider, params object[] parameters)
        {
            return ActivatorUtilities.CreateInstance<TInstance>(serviceProvider, parameters);
        }
    }
}
