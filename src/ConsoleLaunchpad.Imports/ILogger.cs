namespace ConsoleLaunchpad.Imports
{
    public interface ILogger : IImport
    {
        bool ShouldLog(LogLevel level);

        public bool ShouldLogTrace { get => ShouldLog(LogLevel.Trace); }
        public bool ShouldLogDebug { get => ShouldLog(LogLevel.Debug); }
        public bool ShouldLogInformation { get => ShouldLog(LogLevel.Information); }
        public bool ShouldLogWarning { get => ShouldLog(LogLevel.Warning); }
        public bool ShouldLogError { get => ShouldLog(LogLevel.Error); }
        public bool ShouldLogCritical { get => ShouldLog(LogLevel.Critical); }

        void Log(LogLevel level, string message);
        public void Log(LogLevel level, Exception err);

        public void LogTrace(string message) => Log(LogLevel.Trace, message);
        public void LogDebug(string message) => Log(LogLevel.Debug, message);
        public void LogInformation(string message) => Log(LogLevel.Information, message);
        public void LogWarning(string message) => Log(LogLevel.Warning, message);
        public void LogError(string message) => Log(LogLevel.Error, message);
        public void LogError(Exception err) => Log(LogLevel.Error, err);
        public void LogCritical(string message) => Log(LogLevel.Critical, message);
        public void LogCritical(Exception err) => Log(LogLevel.Error, err);
    }

    public sealed class NullLogger : ILogger
    {
        public bool ShouldLog(LogLevel level) => false;

        public void Log(LogLevel level, string message)
        {
            // Intentionally left blank
        }
        public void Log(LogLevel level, Exception err)
        {
            // Intentionally left blank
        }
    }

    public static class LoggerExtensions
    {
        public static bool IfShouldLog(
            this ILogger logger,
            LogLevel level,
            Func<string> message)
        {
            if (logger.ShouldLog(level))
            {
                logger.Log(level, message());
                return true;
            }
            return false;
        }
        public static bool IfShouldLog(
            this ILogger logger,
            LogLevel level,
            Func<Exception> err)
        {
            if (logger.ShouldLog(level))
            {
                logger.Log(level, err());
                return true;
            }
            return false;
        }

        public static bool IfShouldLogTrace(this ILogger logger, Func<string> message) => logger.IfShouldLog(LogLevel.Trace, message);
        public static bool IfShouldLogDebug(this ILogger logger, Func<string> message) => logger.IfShouldLog(LogLevel.Debug, message);
        public static bool IfShouldLogInformation(this ILogger logger, Func<string> message) => logger.IfShouldLog(LogLevel.Information, message);
        public static bool IfShouldLogWarning(this ILogger logger, Func<string> message) => logger.IfShouldLog(LogLevel.Warning, message);
        public static bool IfShouldLogError(this ILogger logger, Func<string> message) => logger.IfShouldLog(LogLevel.Error, message);
        public static bool IfShouldLogError(this ILogger logger, Func<Exception> err) => logger.IfShouldLog(LogLevel.Error, err);
        public static bool IfShouldLogCritical(this ILogger logger, Func<string> message) => logger.IfShouldLog(LogLevel.Critical, message);
        public static bool IfShouldLogCritical(this ILogger logger, Func<Exception> err) => logger.IfShouldLog(LogLevel.Error, err);

        public static bool ShouldLog(this LogLevel currentLevel, LogLevel desiredLevel) => currentLevel >= desiredLevel;
    }

    public enum LogLevel
    {
        Trace,
        Debug,
        Information,
        Warning,
        Error,
        Critical,
        None
    }
}
