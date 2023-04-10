using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rhisis.Protocol.Generators.Constants;
using Rhisis.Protocol.Generators.Extensions;
using Rhisis.Protocol.Generators.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Protocol.Generators;

public class PacketDispatcherCodeGenerator
{
    private readonly GeneratorExecutionContext _context;
    private readonly string _className;
    private readonly IEnumerable<PacketHandlerObject> _handlers;
    private readonly string _packetTypeName;

    public PacketDispatcherCodeGenerator(GeneratorExecutionContext context, string className, IEnumerable<PacketHandlerObject> handlers, string type)
    {
        _context = context;
        _className = className;
        _handlers = handlers;
        _packetTypeName = type;
    }

    public string GenerateCode()
    {
        NamespaceDeclarationSyntax @namespace = SyntaxFactory.NamespaceDeclaration(
            SyntaxFactory.IdentifierName(_context.Compilation.AssemblyName))
            .WithMembers(
                SyntaxFactory.SingletonList<MemberDeclarationSyntax>(
                    SyntaxFactory.ClassDeclaration(_className)
                        .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                        .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                        .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword))
                        .AddMembers(
                            CreateOnBeforeExecuteMethod(),
                            CreateOnAfterExecuteMethod(),
                            CreateOnHandlerNotImplementedMethod(),
                            CreateExecuteMethod(_handlers)
                        )
                )
            );

        return SyntaxFactory.CompilationUnit()
            .AddMembers(@namespace)
            .NormalizeWhitespace()
            .ToFullString();
    }

    /// <summary>
    /// Creates the "OnBeforeExecuteMethod()" method.
    /// </summary>
    /// <remarks>
    /// Output:
    /// static partial void OnBeforeExecute(FFConnection, object)
    /// </remarks>
    /// <returns>OnBeforeExecute() method declaration syntax.</returns>
    private MemberDeclarationSyntax CreateOnBeforeExecuteMethod()
    {
        return SyntaxFactory
            .MethodDeclaration(SyntaxFactory.ParseTypeName("void"), PacketDispatcherConstants.OnBeforeExecuteMethodName)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword))
            .WithParameterList(
                SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList<ParameterSyntax>(new[]
                    {
                        SyntaxFactory
                            .Parameter(SyntaxFactory.Identifier("user"))
                            .WithType(SyntaxFactory.ParseTypeName(PacketDispatcherConstants.FFUserConnectionTypeName)),
                        SyntaxFactory
                            .Parameter(SyntaxFactory.Identifier("handler"))
                            .WithType(SyntaxFactory.ParseTypeName("object"))
                    })
                )
             )
            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
    }

    /// <summary>
    /// Creates the "OnAfterExecute()" method.
    /// </summary>
    /// <remarks>
    /// Output:
    /// static partial void OnAfterExecute(FFConnection, object)
    /// </remarks>
    /// <returns>OnAfterExecute() method declaration syntax.</returns>
    private MemberDeclarationSyntax CreateOnAfterExecuteMethod()
    {
        return SyntaxFactory
            .MethodDeclaration(SyntaxFactory.ParseTypeName("void"), PacketDispatcherConstants.OnAfterExecuteMethodName)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword))
            .WithParameterList(
                SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList<ParameterSyntax>(new[]
                    {
                        SyntaxFactory
                            .Parameter(SyntaxFactory.Identifier("user"))
                            .WithType(SyntaxFactory.ParseTypeName(PacketDispatcherConstants.FFUserConnectionTypeName)),
                        SyntaxFactory
                            .Parameter(SyntaxFactory.Identifier("handler"))
                            .WithType(SyntaxFactory.ParseTypeName("object"))
                    })
                )
             )
            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
    }

    /// <summary>
    /// Creates the "OnHandlerNotImplemented()" method.
    /// </summary>
    /// <remarks>
    /// Output:
    /// static partial void OnHandlerNotImplemented(PacketType)
    /// </remarks>
    /// <returns>OnHandlerNotImplemented() method declaration syntax.</returns>
    private MemberDeclarationSyntax CreateOnHandlerNotImplementedMethod()
    {
        return SyntaxFactory
            .MethodDeclaration(SyntaxFactory.ParseTypeName("void"), PacketDispatcherConstants.OnHandlerNotImplemented)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword))

            .WithParameterList(
                SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList<ParameterSyntax>(new[]
                    {
                        SyntaxFactory
                            .Parameter(SyntaxFactory.Identifier("user"))
                            .WithType(SyntaxFactory.ParseTypeName(PacketDispatcherConstants.FFUserConnectionTypeName)),
                        SyntaxFactory
                            .Parameter(SyntaxFactory.Identifier("packetType"))
                            .WithType(SyntaxFactory.ParseTypeName(_packetTypeName))
                    })
                )
             )
            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
    }

    /// <summary>
    /// Creates the "Execute()" method.
    /// </summary>
    /// <remarks>
    /// Output:
    /// static partial void Execute(user, header, packet, serviceProvider)
    /// </remarks>
    /// <returns>Execute() method declaration syntax.</returns>
    private MethodDeclarationSyntax CreateExecuteMethod(IEnumerable<PacketHandlerObject> handlers)
    {
        return SyntaxFactory
            .MethodDeclaration(SyntaxFactory.ParseTypeName("void"), PacketDispatcherConstants.ExecuteMethodName)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
            .WithParameterList(
                SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList<ParameterSyntax>(new[]
                    {
                        SyntaxFactory
                            .Parameter(SyntaxFactory.Identifier("user"))
                            .WithType(SyntaxFactory.ParseTypeName(PacketDispatcherConstants.FFUserConnectionTypeName)),
                        SyntaxFactory
                            .Parameter(SyntaxFactory.Identifier("header"))
                            .WithType(SyntaxFactory.ParseName(_packetTypeName)),
                        SyntaxFactory
                            .Parameter(SyntaxFactory.Identifier("packet"))
                            .WithType(SyntaxFactory.ParseName(PacketDispatcherConstants.FFPacketTypeName)),
                        SyntaxFactory
                            .Parameter(SyntaxFactory.Identifier("serviceProvider"))
                            .WithType(SyntaxFactory.ParseName(PacketDispatcherConstants.ServiceProviderTypeName))
                    })
                )
            )
            .WithBody(
                SyntaxFactory.Block(
                    SyntaxFactory.SingletonList<StatementSyntax>(
                        SyntaxFactory.SwitchStatement(
                            SyntaxFactory.IdentifierName("header")
                        )
                        .WithSections(
                            SyntaxFactory.List<SwitchSectionSyntax>(GenerateSwitchSections(handlers))
                        )
                    )
                )
            );
    }

    private IEnumerable<SwitchSectionSyntax> GenerateSwitchSections(IEnumerable<PacketHandlerObject> handlers)
    {
        var handlerSections = handlers
            .Where(x => x.IsValid)
            .Select(x =>
                SyntaxFactory
                    .SwitchSection()
                    .WithLabels(
                        SyntaxFactory.SingletonList<SwitchLabelSyntax>(
                            SyntaxFactory.CaseSwitchLabel(
                                SyntaxFactory.IdentifierName($"{PacketDispatcherConstants.ProtocolNamspace}.{x.PacketType}")
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
                                        SyntaxFactory.IdentifierName(AssemblySymbolDefinitions.GetFullType(x.PacketMessageClassName))
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
                                        SyntaxFactory.IdentifierName(AssemblySymbolDefinitions.GetFullType(x.ClassName))
                                    )
                                    .WithVariables(
                                        SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                            SyntaxFactory
                                                .VariableDeclarator(x.ClassName.ToCamelCase())
                                                .WithInitializer(
                                                    SyntaxFactory.EqualsValueClause(
                                                        SyntaxFactory.CastExpression(SyntaxFactory.IdentifierName(AssemblySymbolDefinitions.GetFullType(x.ClassName)),
                                                            SyntaxFactory.InvocationExpression(
                                                                SyntaxFactory.MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    SyntaxFactory.IdentifierName(PacketDispatcherConstants.ActivatorUtilitiesClassName),
                                                                    SyntaxFactory.IdentifierName(PacketDispatcherConstants.CreateInstanceMethodName)
                                                                )
                                                            )
                                                            .WithArgumentList(
                                                                SyntaxFactory.ArgumentList(
                                                                    SyntaxFactory.SeparatedList<ArgumentSyntax>(new[]
                                                                    {
                                                                        SyntaxFactory.Argument(SyntaxFactory.IdentifierName("serviceProvider")),
                                                                        SyntaxFactory.Argument(SyntaxFactory.TypeOfExpression(SyntaxFactory.IdentifierName(AssemblySymbolDefinitions.GetFullType(x.ClassName))))
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
                                    SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName(PacketDispatcherConstants.OnBeforeExecuteMethodName))
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
                                            SyntaxFactory.IdentifierName(PacketDispatcherConstants.ExecuteMethodName)
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
                                    SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName(PacketDispatcherConstants.OnAfterExecuteMethodName))
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
                                SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName(PacketDispatcherConstants.OnHandlerNotImplemented))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SeparatedList<ArgumentSyntax>(new[]
                                            {
                                                SyntaxFactory.Argument(SyntaxFactory.IdentifierName("user")),
                                                SyntaxFactory.Argument(SyntaxFactory.IdentifierName("header"))
                                            })
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
