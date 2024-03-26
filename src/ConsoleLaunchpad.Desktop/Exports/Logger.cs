using System;
using System.Composition;
using System.Diagnostics;
using ConsoleLaunchpad.Imports;

namespace ConsoleLaunchpad.Desktop.Exports
{
    [Export(typeof(ILogger))]
    [Shared]
    public class DesktopLogger : ILogger
    {
        public bool ShouldLog(LogLevel level) => level.ShouldLog(App.Config.LogLevel);
        public void Log(LogLevel level, string message)
        {
            var messageTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var foreground = Console.ForegroundColor;
            switch (level)
            {
                case LogLevel.None:
                    break;
                case LogLevel.Trace:
                    Trace.WriteLine($"{messageTime} - {message}");
                    break;
                case LogLevel.Debug:
                    Debug.WriteLine($"{messageTime} - {message}");
                    break;
                case LogLevel.Information:
#if DEBUG
                    if (Debugger.IsAttached) Debug.WriteLine($"{messageTime} - {message}");
#endif
                    Console.WriteLine($"{messageTime} - {message}");
                    break;
                case LogLevel.Warning:
#if DEBUG
                    if (Debugger.IsAttached) Debug.WriteLine($"{messageTime} - {message}");
#endif
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{messageTime} - {message}");
                    Console.ForegroundColor = foreground;
                    break;
                case LogLevel.Error:
#if DEBUG
                    if (Debugger.IsAttached) Debug.WriteLine($"{messageTime} - {message}");
#endif
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{messageTime} - {message}");
                    Console.ForegroundColor = foreground;
                    break;
                case LogLevel.Critical:
#if DEBUG
                    if (Debugger.IsAttached) Debug.WriteLine($"{messageTime} - {message}");
#endif
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"{messageTime} - {message}");
                    Console.ForegroundColor = foreground;
                    break;
            }
        }
        public void Log(LogLevel level, Exception err) => Log(level, err.ToString());
    }
}
