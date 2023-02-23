using Microsoft.CodeAnalysis;
using Rhisis.Protocol.Generators.Helpers;

namespace Rhisis.Protocol.Generators;

[Generator]
public class PacketTypeDispatcherGenerator : PacketDispatcherGeneratorBase, ISourceGenerator
{
    public PacketTypeDispatcherGenerator()
        : base(PacketDispatcherConstants.PacketTypeName)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        PacketHandlerSyntaxReceiver syntaxReceiver = context.SyntaxReceiver as PacketHandlerSyntaxReceiver;

        if (syntaxReceiver is null)
        {
            return;
        }

        AssemblySymbolDefinitions.Load(context.Compilation.SourceModule);
        string code = GenerateCode(context, PacketDispatcherConstants.PacketDispatcherClassName, syntaxReceiver.Handlers);

        // Add the source code to the compilation
        context.AddSource($"PacketTypeDispatcher.g.cs", code);
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new PacketHandlerSyntaxReceiver(PacketDispatcherConstants.PacketHandlerAttributeName));
    }
}
