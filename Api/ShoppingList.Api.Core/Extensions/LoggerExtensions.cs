using Microsoft.Extensions.Logging;

namespace ProjectHermes.ShoppingList.Api.Core.Extensions;

public static class LoggerExtensions
{
    public static void LogTrace<T>(this ILogger<T> logger, Func<string> logFunction)
    {
        if (!logger.IsEnabled(LogLevel.Trace))
            return;

        logger.LogTrace(logFunction());
    }

    public static void LogDebug<T>(this ILogger<T> logger, Func<string> logFunction)
    {
        if (!logger.IsEnabled(LogLevel.Debug))
            return;

        logger.LogDebug(logFunction());
    }

    public static void LogInformation<T>(this ILogger<T> logger, Func<string> logFunction)
    {
        if (!logger.IsEnabled(LogLevel.Information))
            return;

        logger.LogInformation(logFunction());
    }

    public static void LogWarning<T>(this ILogger<T> logger, Func<string> logFunction)
    {
        if (!logger.IsEnabled(LogLevel.Warning))
            return;

        logger.LogWarning(logFunction());
    }

    public static void LogWarning<T>(this ILogger<T> logger, Exception ex, Func<string> logFunction)
    {
        if (!logger.IsEnabled(LogLevel.Warning))
            return;

        logger.LogWarning(ex, logFunction());
    }
}