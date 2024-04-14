using Microsoft.Extensions.Logging;

#pragma warning disable CA2254

namespace ProjectHermes.Xipona.Api.Core.Extensions;

public static class LoggerExtensions
{
    public static void LogTrace<T>(this ILogger<T> logger, Func<string> message, params object[] args)
    {
        if (!logger.IsEnabled(LogLevel.Trace))
            return;

        logger.LogTrace(message(), args);
    }

    public static void LogDebug<T>(this ILogger<T> logger, Func<string> message, params object[] args)
    {
        if (!logger.IsEnabled(LogLevel.Debug))
            return;

        logger.LogDebug(message(), args);
    }

    public static void LogInformation<T>(this ILogger<T> logger, Exception e, Func<string> message, params object[] args)
    {
        if (!logger.IsEnabled(LogLevel.Information))
            return;

        logger.LogInformation(e, message(), args);
    }

    public static void LogInformation<T>(this ILogger<T> logger, Func<string> message, params object[] args)
    {
        if (!logger.IsEnabled(LogLevel.Information))
            return;

        logger.LogInformation(message(), args);
    }

    public static void LogWarning<T>(this ILogger<T> logger, Func<string> message, params object[] args)
    {
        if (!logger.IsEnabled(LogLevel.Warning))
            return;

        logger.LogWarning(message(), args);
    }

    public static void LogWarning<T>(this ILogger<T> logger, Exception ex, Func<string> message, params object[] args)
    {
        if (!logger.IsEnabled(LogLevel.Warning))
            return;

        logger.LogWarning(ex, message(), args);
    }

    public static void LogError<T>(this ILogger<T> logger, Exception ex, Func<string> message, params object[] args)
    {
        if (!logger.IsEnabled(LogLevel.Error))
            return;

        logger.LogError(ex, message(), args);
    }

    public static void LogError<T>(this ILogger<T> logger, Func<string> message, params object[] args)
    {
        if (!logger.IsEnabled(LogLevel.Error))
            return;

        logger.LogError(message(), args);
    }
}