namespace ConsoleLaunchpad.Imports
{
    public interface ISettings : IImport, IDisposable
    {
        public void SaveSettings();
        public void LoadSettings();

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                SaveSettings();
            }
        }
    }

    public sealed class NullSettings : ISettings
    {
        public void SaveSettings()
        {
            // Intentionally left blank
        }
        public void LoadSettings()
        {
            // Intentionally left blank
        }
    }
}
