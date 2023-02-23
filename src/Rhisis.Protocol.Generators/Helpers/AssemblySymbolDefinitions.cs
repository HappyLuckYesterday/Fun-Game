using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Protocol.Generators.Helpers;

internal static class AssemblySymbolDefinitions
{
    private static IEnumerable<ISymbol> _symbols;

    public static void Load(IModuleSymbol module)
    {
        List<IAssemblySymbol> assemblySymbols = new()
        {
            module.ContainingSymbol as IAssemblySymbol
        };
        assemblySymbols.AddRange(module.ReferencedAssemblySymbols.Where(x => x.Name.StartsWith("Rhisis")));

        _symbols = assemblySymbols.SelectMany(x =>
        {
            try
            {
                var main = x.Identity.Name.Split('.').Aggregate(x.GlobalNamespace, (s, c) => s.GetNamespaceMembers().Single(m => m.Name.Equals(c)));

                return GetAllTypes(main);
            }
            catch
            {
                return Enumerable.Empty<ITypeSymbol>();
            }
        }).ToList();
    }

    public static string GetFullType(string type)
    {
        return GetSymbol(type).ToString();
    }

    public static string GetNamespace(string type)
    {
        return GetSymbol(type).ContainingNamespace.ToString();
    }

    private static ISymbol GetSymbol(string type)
    {
        ISymbol symbol = _symbols.FirstOrDefault(x => x.Name.Equals(type));

        if (symbol is null)
        {
            throw new ArgumentException($"Failed to find type symbol for '{type}'.");
        }

        return symbol;
    }

    private static IEnumerable<ITypeSymbol> GetAllTypes(INamespaceSymbol root)
    {
        foreach (var namespaceOrTypeSymbol in root.GetMembers())
        {
            if (namespaceOrTypeSymbol is INamespaceSymbol @namespace)
            {
                foreach (var nested in GetAllTypes(@namespace))
                {
                    yield return nested;
                }
            }
            else if (namespaceOrTypeSymbol is ITypeSymbol type)
            {
                yield return type;
            }
        }
    }
}