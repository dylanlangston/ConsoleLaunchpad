using System;
using System.Composition;
using System.Diagnostics;
using ConsoleLaunchpad.Imports;

namespace ConsoleLaunchpad.Desktop.Exports
{
    [Export(typeof(ISettings))]
    [Shared]
    public sealed class NullSettings : ISettings
    {
        public void SaveSettings()
        {
            Console.WriteLine("Settings Saved");
        }
        public void LoadSettings()
        {
            Console.WriteLine("Settings Loaded");
        }
    }
}
