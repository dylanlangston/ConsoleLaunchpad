using ConsoleLaunchpad.Imports;

namespace ConsoleLaunchpad.ViewModels;

public class MainViewModel : ViewModelBase
{
    public IViewModelBase _currentView = new ProfilesViewModel();
    public IViewModelBase CurrentView
    {
        get => _currentView;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _currentView.Dispose();
        }
    }
}
