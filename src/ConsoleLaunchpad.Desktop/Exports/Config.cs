using System;
using System.Composition;
using System.Diagnostics;
using ConsoleLaunchpad.Imports;

namespace ConsoleLaunchpad.Desktop.Exports
{
    [Export(typeof(IConfig))]
    [Shared]
    public sealed class DesktopConfig : IConfig
    {
        public PlatformType PlatformType { get => PlatformType.Desktop; }

#if DEBUG
        public LogLevel LogLevel { get => LogLevel.Trace; }
#else
        public LogLevel LogLevel { get => LogLevel.Critical; }
#endif    
    }
}
