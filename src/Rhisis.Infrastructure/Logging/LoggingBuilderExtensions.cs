using Microsoft.Extensions.Logging;

namespace Rhisis.Infrastructure.Logging;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddLoggingFilters(this ILoggingBuilder builder)
    {
        builder.SetMinimumLevel(LogLevel.Trace);
        builder.AddFilter("LiteNetwork.*", LogLevel.Warning);
        builder.AddFilter("Microsoft.EntityFrameworkCore.*", LogLevel.Warning);
        builder.AddFilter("Microsoft.Extensions.*", LogLevel.Warning);
        builder.AddFilter("Microsoft.Hosting.*", LogLevel.Warning);

        return builder;
    }
}
