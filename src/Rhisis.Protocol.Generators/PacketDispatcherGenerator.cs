using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rhisis.Protocol.Generators.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Protocol.Generators;

[Generator]
public class PacketDispatcherGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        PacketHandlerSyntaxReceiver syntaxReceiver = context.SyntaxReceiver as PacketHandlerSyntaxReceiver;

        if (syntaxReceiver is null)
        {
            return;
        }

        List<IAssemblySymbol> assemblySymbols = new()
        {
            context.Compilation.SourceModule.ContainingSymbol as IAssemblySymbol
        };
        assemblySymbols.AddRange(context.Compilation.SourceModule.ReferencedAssemblySymbols.Where(x => x.Name.StartsWith("Rhisis")));


        AssembliesSymbolDefinition symbols = new(assemblySymbols);

        ClassDeclarationSyntax @class = SyntaxFactory.ClassDeclaration("PacketDispatcher")
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword))
            .AddMembers(
                // static partial void OnBeforeExecute(IPacketHandler)
                SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "OnBeforeExecute")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword))
                    .WithParameterList(
                        SyntaxFactory.ParameterList(
                            SyntaxFactory.SeparatedList<ParameterSyntax>(new[]
                            {
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("user")).WithType(SyntaxFactory.ParseTypeName(symbols.GetFullType("FFUserConnection"))),
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("handler")).WithType(SyntaxFactory.ParseTypeName(symbols.GetFullType("IPacketHandler")))
                            })
                        )
                     )
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),

                // static partial void OnAfterExecute(IPacketHandler)
                SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "OnAfterExecute")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword))
                    .WithParameterList(
                        SyntaxFactory.ParameterList(
                            SyntaxFactory.SeparatedList<ParameterSyntax>(new[]
                            {
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("user")).WithType(SyntaxFactory.ParseTypeName(symbols.GetFullType("FFUserConnection"))),
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("handler")).WithType(SyntaxFactory.ParseTypeName(symbols.GetFullType("IPacketHandler")))
                            })
                        )
                     )
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),

                // static partial HandlerNotImplemented(PacketType)
                SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "HandlerNotImplemented")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword))
                    .WithParameterList(
                        SyntaxFactory.ParameterList(
                            SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory
                                    .Parameter(SyntaxFactory.Identifier("packetType"))
                                    .WithType(SyntaxFactory.ParseTypeName(symbols.GetFullType("PacketType")))
                            )
                        )
                     )
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),

                // public static void Execute(FFConnection, FFPacket, IServiceProvider)
                SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "Execute")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                    .WithParameterList(
                        SyntaxFactory.ParameterList(
                            SyntaxFactory.SeparatedList<ParameterSyntax>(new[]
                            {
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("user")).WithType(SyntaxFactory.ParseTypeName(symbols.GetFullType("FFUserConnection"))),
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("packet")).WithType(SyntaxFactory.ParseName(symbols.GetFullType("FFPacket"))),
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("serviceProvider")).WithType(SyntaxFactory.ParseName("System.IServiceProvider"))
                            })
                        )
                    )
                    .WithBody(
                        SyntaxFactory.Block(
                            SyntaxFactory.SingletonList<StatementSyntax>(
                                SyntaxFactory.SwitchStatement(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression, 
                                        SyntaxFactory.IdentifierName("packet"), 
                                        SyntaxFactory.IdentifierName("Header")
                                    )
                                )
                                .WithSections(
                                    SyntaxFactory.List<SwitchSectionSyntax>(GenerateSwitchSections(syntaxReceiver.Handlers, symbols))
                                )
                            )
                        )
                    )
            );

        NamespaceDeclarationSyntax @namespace = SyntaxFactory.NamespaceDeclaration(
            SyntaxFactory.IdentifierName(context.Compilation.AssemblyName))
            .WithMembers(
                SyntaxFactory.SingletonList<MemberDeclarationSyntax>(@class)
            );

        string code = SyntaxFactory.CompilationUnit()
            .AddMembers(@namespace)
            .NormalizeWhitespace()
            .ToFullString();

        // Add the source code to the compilation
        context.AddSource($"PacketDispatcher.g.cs", code);
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new PacketHandlerSyntaxReceiver());
    }

    private static IEnumerable<SwitchSectionSyntax> GenerateSwitchSections(IEnumerable<PacketHandlerObject> handlers, AssembliesSymbolDefinition symbols)
    {
        var handlerSections = handlers
            .Select(x => 
                SyntaxFactory
                    .SwitchSection()
                    .WithLabels(
                        SyntaxFactory.SingletonList<SwitchLabelSyntax>(
                            SyntaxFactory.CaseSwitchLabel(
                                SyntaxFactory.IdentifierName($"{symbols.GetNamespace("PacketType")}.{x.PacketType}")
                            )
                        )
                    )
                    .WithStatements(
                        SyntaxFactory.List<StatementSyntax>(
                            new StatementSyntax[]
                            {
                                // PACKET_OBJECT p = new(packet);
                                SyntaxFactory.LocalDeclarationStatement(
                                    SyntaxFactory.VariableDeclaration(
                                        SyntaxFactory.IdentifierName(symbols.GetFullType(x.PacketMessageClassName))
                                    )
                                    .WithVariables(
                                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                            SyntaxFactory
                                                .VariableDeclarator(x.PacketMessageClassName.ToCamelCase())
                                                .WithInitializer(
                                                    SyntaxFactory.EqualsValueClause(
                                                        SyntaxFactory
                                                            .ImplicitObjectCreationExpression()
                                                            .WithArgumentList(
                                                                SyntaxFactory.ArgumentList(
                                                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                        SyntaxFactory.Argument(SyntaxFactory.IdentifierName("packet"))
                                                                    )
                                                                )
                                                            )
                                                    )
                                                )
                                        )
                                    )
                                ),

                                // PACKET_HANDLER handler = ActivatorUtilities.CreateInstance(serviceProvider, typeof(PACKET_HANDLER)) as PACKET_HANDLER;
                                SyntaxFactory.LocalDeclarationStatement(
                                    SyntaxFactory.VariableDeclaration(
                                        SyntaxFactory.IdentifierName(symbols.GetFullType(x.ClassName))
                                    )
                                    .WithVariables(
                                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                            SyntaxFactory
                                                .VariableDeclarator(x.ClassName.ToCamelCase())
                                                .WithInitializer(
                                                    SyntaxFactory.EqualsValueClause(
                                                        SyntaxFactory.CastExpression(SyntaxFactory.IdentifierName(symbols.GetFullType(x.ClassName)),
                                                            SyntaxFactory.InvocationExpression(
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.IdentifierName("Microsoft.Extensions.DependencyInjection.ActivatorUtilities"),
                                                                    SyntaxFactory.IdentifierName("CreateInstance")
                                                                )
                                                            )
                                                            .WithArgumentList(
                                                                SyntaxFactory.ArgumentList(
                                                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(new[]
                                                                    {
                                                                        SyntaxFactory.Argument(SyntaxFactory.IdentifierName("serviceProvider")),
                                                                        SyntaxFactory.Argument(SyntaxFactory.TypeOfExpression(SyntaxFactory.IdentifierName(symbols.GetFullType(x.ClassName))))
                                                                    })
                                                                )
                                                            )
                                                        )
                                                    )
                                                )
                                        )
                                    )
                                ),

                                // OnBeforeExecute(PACKET_HANDLER);
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName("OnBeforeExecute"))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SeparatedList<ArgumentSyntax>(new[]
                                            {
                                                SyntaxFactory.Argument(SyntaxFactory.IdentifierName("user")),
                                                SyntaxFactory.Argument(SyntaxFactory.IdentifierName(x.ClassName.ToCamelCase()))
                                            })
                                        )
                                    )
                                ),
                                // PACKET_HANDLER.Execute(PACKET_OBJECT);
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName(x.ClassName.ToCamelCase()),
                                            SyntaxFactory.IdentifierName("Execute")
                                        )
                                    )
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(SyntaxFactory.IdentifierName(x.PacketMessageClassName.ToCamelCase()))
                                            )
                                        )
                                    )
                                ),
                                
                                // OnAfterExecute(PACKET_HANDLER);
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName("OnAfterExecute"))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SeparatedList<ArgumentSyntax>(new[]
                                            {
                                                SyntaxFactory.Argument(SyntaxFactory.IdentifierName("user")),
                                                SyntaxFactory.Argument(SyntaxFactory.IdentifierName(x.ClassName.ToCamelCase()))
                                            })
                                        )
                                    )
                                ),
                                SyntaxFactory.BreakStatement()
                            }
                        )
                    )
            );

        return handlerSections.Concat(
            new[]
            {
                SyntaxFactory.SwitchSection()
                .WithLabels(
                    SyntaxFactory.SingletonList<SwitchLabelSyntax>(
                        SyntaxFactory.DefaultSwitchLabel()
                    )
                )
                .WithStatements(
                    SyntaxFactory.List<StatementSyntax>(
                        new StatementSyntax[]
                        {
                            SyntaxFactory.ExpressionStatement(
                                SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName("HandlerNotImplemented"))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.IdentifierName("packet"), 
                                                        SyntaxFactory.IdentifierName("Header")
                                                    )
                                                )
                                            )
                                        )
                                    )
                            ),
                            SyntaxFactory.BreakStatement()
                        }
                    )
                )
            }
        );
    }
}

internal sealed class AssembliesSymbolDefinition
{
    private readonly IEnumerable<ISymbol> _symbols;

    public AssembliesSymbolDefinition(IEnumerable<IAssemblySymbol> assemblySymbols)
    {
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

    public string GetFullType(string type)
    {
        return GetSymbol(type).ToString();
    }

    public string GetNamespace(string type)
    {
        return GetSymbol(type).ContainingNamespace.ToString();
    }

    private ISymbol GetSymbol(string type)
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