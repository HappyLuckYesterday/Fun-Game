using Microsoft.CodeAnalysis;

namespace Rhisis.Protocol.Generators.Constants;

internal static class Diagnostics
{
    public static DiagnosticDescriptor MissingExecuteMethod(string packetType)
    {
        return new("RHIGEN001",
            title: "Missing Execute method",
            messageFormat: $"No execute method found for packet handler '{packetType}'.",
            category: "Packet Dispatcher Generator",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);
    }
}