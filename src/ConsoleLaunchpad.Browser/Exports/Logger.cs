using ConsoleLaunchpad.Imports;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLaunchpad.Browser.Exports
{
    [Export(typeof(ILogger))]
    [Shared]
    public partial class BrowserLogger : ILogger
    {
        [JSImport("globalThis.console.log")]
        internal static partial void Log([JSMarshalAs<JSType.String>] string message);
        [JSImport("globalThis.console.info")]
        internal static partial void Info([JSMarshalAs<JSType.String>] string message);
        [JSImport("globalThis.console.warn")]
        internal static partial void Warn([JSMarshalAs<JSType.String>] string message);
        [JSImport("globalThis.console.debug")]
        internal static partial void Debug([JSMarshalAs<JSType.String>] string message);
        [JSImport("globalThis.console.error")]
        internal static partial void Error([JSMarshalAs<JSType.String>] string message);
        [JSImport("globalThis.console.trace")]
        internal static partial void Trace([JSMarshalAs<JSType.String>] string message);


        public bool ShouldLog(LogLevel level) => level.ShouldLog(App.Config.LogLevel);
        public void Log(LogLevel level, string message)
        {
            var messageTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            switch (level)
            {
                case LogLevel.None:
                    break;
                case LogLevel.Trace:
                    Trace($"{messageTime} - {message}");
                    break;
                case LogLevel.Debug:
                    Debug($"{messageTime} - {message}");
                    break;
                case LogLevel.Information:
                    Info($"{messageTime} - {message}");
                    break;
                case LogLevel.Warning:
                    Warn($"{messageTime} - {message}");
                    break;
                case LogLevel.Error:
                    Error($"{messageTime} - {message}");
                    break;
                case LogLevel.Critical:
                    Error($"{messageTime} - {message}");
                    break;
            }
        }
        public void Log(LogLevel level, Exception err) => Log(level, err.ToString());
    }
}