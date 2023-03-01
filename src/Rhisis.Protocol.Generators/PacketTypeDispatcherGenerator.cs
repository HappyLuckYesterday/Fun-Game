using Microsoft.CodeAnalysis;
using Rhisis.Protocol.Generators.Constants;
using Rhisis.Protocol.Generators.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Protocol.Generators;

[Generator]
public class PacketTypeDispatcherGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        PacketHandlerSyntaxReceiver syntaxReceiver = context.SyntaxReceiver as PacketHandlerSyntaxReceiver;

        if (syntaxReceiver is null)
        {
            return;
        }

        foreach (PacketHandlerObject handler in syntaxReceiver.Handlers)
        {
            if (!handler.HasExecuteMethod)
            {
                context.ReportDiagnostic(Diagnostic.Create(Diagnostics.MissingExecuteMethod(handler.PacketType), handler.ClassNode.GetLocation()));
            }
        }

        AssemblySymbolDefinitions.Load(context.Compilation.SourceModule);
        string packetDispatcherCode = GeneratePacketDispatcherCode(context, syntaxReceiver.Handlers.Where(x => x.PacketType.Contains("PacketType")));
        string snapshotDispatcherCode = GenerateSnapshotDispatcherCode(context, syntaxReceiver.Handlers.Where(x => x.PacketType.Contains("SnapshotType")));

        // Add the source code to the compilation
        context.AddSource($"PacketTypeDispatcher.g.cs", packetDispatcherCode);
        context.AddSource($"SnapshotTypeDispatcher.g.cs", snapshotDispatcherCode);
    }

    private string GeneratePacketDispatcherCode(GeneratorExecutionContext context, IEnumerable<PacketHandlerObject> handlers)
    {
        PacketDispatcherCodeGenerator generator = new(context, PacketDispatcherConstants.PacketDispatcherClassName, handlers, PacketDispatcherConstants.PacketTypeName);

        return generator.GenerateCode();
    }

    private string GenerateSnapshotDispatcherCode(GeneratorExecutionContext context, IEnumerable<PacketHandlerObject> handlers)
    {
        PacketDispatcherCodeGenerator generator = new(context, PacketDispatcherConstants.SnapshotDispatcherClassName, handlers, PacketDispatcherConstants.SnapshotTypeName);

        return generator.GenerateCode();
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new PacketHandlerSyntaxReceiver());
    }
}
