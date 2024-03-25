namespace ConsoleLaunchpad.Imports
{
    public interface IConfig : IImport
    {
        public PlatformType PlatformType { get; }

        public LogLevel LogLevel { get; }
    }

    public sealed class NullConfig : IConfig
    {
        public PlatformType PlatformType { get => PlatformType.Unknown; }

        public LogLevel LogLevel { get => LogLevel.None; }
    }

    public enum PlatformType
    {
        Unknown,
        Browser,
        Desktop
    }
}
