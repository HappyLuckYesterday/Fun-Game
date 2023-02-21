using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace Rhisis.Protocol.Generators;

[DebuggerDisplay("{ClassName}")]
internal sealed class PacketHandlerObject
{
    public ClassDeclarationSyntax ClassNode { get; }

    public string ClassName => ClassNode.Identifier.ValueText;

    public string PacketType { get; set; }

    public string PacketMessageClassName { get; set; }

    public bool HasExecuteMethod { get; set; }

    public PacketHandlerObject(ClassDeclarationSyntax classNode)
    {
        ClassNode = classNode;
    }
}