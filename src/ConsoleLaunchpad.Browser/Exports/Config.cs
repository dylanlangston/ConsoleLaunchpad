using ConsoleLaunchpad.Imports;
using System.Composition;

namespace ConsoleLaunchpad.Browser.Exports
{
    [Export(typeof(IConfig))]
    [Shared]
    public class Config : IConfig
    {
        public PlatformType PlatformType { get => PlatformType.Browser; }

#if DEBUG
        public LogLevel LogLevel { get => LogLevel.Trace; }
#else
        public LogLevel LogLevel { get => LogLevel.Critical; }
#endif
    }
}