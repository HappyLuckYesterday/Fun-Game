using Microsoft.CodeAnalysis;
using Rhisis.Protocol.Generators.Constants;
using Rhisis.Protocol.Generators.Helpers;
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

        foreach (PacketHandlerObject handler in syntaxReceiver.Handlers)
        {
            if (!handler.HasExecuteMethod)
            {
                context.ReportDiagnostic(Diagnostic.Create(Diagnostics.MissingExecuteMethod(handler.PacketType), handler.ClassNode.GetLocation()));
            }
        }

        AssemblySymbolDefinitions.Load(context.Compilation.SourceModule);
        string packetDispatcherCode = GeneratePacketDispatcherCode(context, syntaxReceiver.Handlers.Where(x => x.PacketType.Contains("PacketType")));
        string corePacketDispatcherCode = GeneratePacketDispatcherCode(context, syntaxReceiver.Handlers.Where(x => x.PacketType.Contains("CorePacketType")));

        // Add the source code to the compilation
        if (!string.IsNullOrWhiteSpace(packetDispatcherCode))
        {
            context.AddSource($"PacketTypeDispatcher.g.cs", packetDispatcherCode);
        }

        if (!string.IsNullOrWhiteSpace(corePacketDispatcherCode))
        {
            context.AddSource($"CorePacketTypeDispatcher.g.cs", corePacketDispatcherCode);
        }
    }

    private string GeneratePacketDispatcherCode(GeneratorExecutionContext context, IEnumerable<PacketHandlerObject> handlers)
    {
        if (!handlers.Any())
        {
            return null;
        }

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
