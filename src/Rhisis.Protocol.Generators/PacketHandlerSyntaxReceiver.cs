using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rhisis.Protocol.Generators.Constants;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Protocol.Generators;

internal class PacketHandlerSyntaxReceiver : ISyntaxReceiver
{
    private readonly List<PacketHandlerObject> _packetHandler = new();
    private readonly string[] _attributeNames = new[]
    {
        PacketDispatcherConstants.PacketHandlerAttributeName
    };

    public IReadOnlyList<PacketHandlerObject> Handlers => _packetHandler;

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not null and ClassDeclarationSyntax @class)
        {
            if (@class.AttributeLists.Any())
            {
                PacketHandlerObject packetHandler = new(@class);
                AttributeSyntax packetHandlerAttribute = @class.AttributeLists.SelectMany(x => x.Attributes).FirstOrDefault(x => _attributeNames.Contains(x.Name.ToString()));

                if (packetHandlerAttribute is not null)
                {
                    packetHandler.PacketType = packetHandlerAttribute.ArgumentList.Arguments.First().ToString();

                    MethodDeclarationSyntax executeMethod = @class.Members.Where(x => x.Kind() == SyntaxKind.MethodDeclaration)
                        .Cast<MethodDeclarationSyntax>()
                        .FirstOrDefault(x => x.Identifier.ValueText == "Execute");

                    if (executeMethod is not null)
                    {
                        packetHandler.HasExecuteMethod = true;
                        packetHandler.PacketMessageClassName = executeMethod.ParameterList.Parameters.FirstOrDefault()?.Type.ToString();
                    }
                }

                _packetHandler.Add(packetHandler);
            }
        }
    }
}
