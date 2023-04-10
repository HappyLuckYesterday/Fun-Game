using Microsoft.EntityFrameworkCore;
using Rhisis.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhisis.Infrastructure.Persistance.Extensions;

internal static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyConfigurationForDbContextFromAssembly<TContext>(this ModelBuilder modelBuilder, Assembly assembly) 
        where TContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(nameof(modelBuilder));
        ArgumentNullException.ThrowIfNull(nameof(assembly));

        MethodInfo applyEntityConfigurationMethod = typeof(ModelBuilder)
            .GetMethods()
            .Single(e => e.Name == nameof(ModelBuilder.ApplyConfiguration) && 
                         e.ContainsGenericParameters && 
                         e.GetParameters().SingleOrDefault()?.ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

        IEnumerable<Type> dbContextDbSetTypes = typeof(TContext).GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
            .Select(x => x.PropertyType.GetGenericArguments().Single());

        IEnumerable<Type> configurationTypes = assembly.GetTypes()
            .Where(x => x.ImplementsInterface(typeof(IEntityTypeConfiguration<>)))
            .ToList();

        foreach (Type configurationType in configurationTypes)
        {
            Type interfaceType = configurationType.GetInterfaces().Single(x => x.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

            if (interfaceType is not null && dbContextDbSetTypes.Contains(interfaceType.GenericTypeArguments[0]))
            {
                MethodInfo target = applyEntityConfigurationMethod.MakeGenericMethod(interfaceType.GenericTypeArguments[0]);

                target.Invoke(modelBuilder, new[] { Activator.CreateInstance(configurationType) });
            }
        }

        return modelBuilder;
    }
}
