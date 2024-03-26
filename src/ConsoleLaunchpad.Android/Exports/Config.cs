using ConsoleLaunchpad.Imports;
using System.Composition;

namespace ConsoleLaunchpad.Android.Exports
{
    [Export(typeof(IConfig))]
    [Shared]
    public class Config : IConfig
    {
        public PlatformType PlatformType { get => PlatformType.Android; }

        public LogLevel LogLevel { get => LogLevel.None; }
    }
}