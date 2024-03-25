using System.Composition;
using Avalonia.Controls;
using ConsoleLaunchpad.Imports;

namespace ConsoleLaunchpad.Exports
{
    // ViewLocator for use in XAML, points to the ViewLocator Singleton Instance
    [Export(typeof(IViewLocator<Control>))]
    [Shared]
    internal class XamlViewLocator : IViewLocator<Control>, IBindableDataTemplate
    {
        public Control? Build(object? data) => ViewLocator.Instance.Build(data);
        public Control? BuildWithContext(IViewModelBase data) => ViewLocator.Instance.BuildWithContext(data);
        public bool Match(object? data) => ViewLocator.Instance.Match(data);
    }
}
