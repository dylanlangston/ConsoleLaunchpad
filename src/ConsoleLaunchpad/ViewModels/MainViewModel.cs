using System;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Avalonia.Controls;
using ConsoleLaunchpad.Imports;
using ReactiveUI;

namespace ConsoleLaunchpad.ViewModels;

public class MainViewModel : ReactiveObject, IViewModelBaseWithDialog, IViewModelBase
{
    public IViewModelBase _currentView = new ProfileViewModel();
    public IViewModelBase CurrentView
    {
        get => _currentView;
    }

    public void OpenErrorDialog(Exception exception, bool fatal)
    {
        var dialogViewModel = new ErrorDialogViewModel(this, exception, !fatal);
        dialogViewModel.OnSubmit += (s, e) =>
        {
            switch (e.Type)
            {
                case ErrorDialogViewModel.ErrorDialogViewModelEventType.Quit:
                    break;
                case ErrorDialogViewModel.ErrorDialogViewModelEventType.Continue:
                    DialogViewModel = null;
                    break;
                default:
                    throw new NotImplementedException();
            }
        };
        DialogViewModel = dialogViewModel;
    }

    #region Dialog
    IViewModelDialogBase? _dialogViewModel;
    public IViewModelDialogBase? DialogViewModel
    {
        get => _dialogViewModel;
        set
        {
            _dialogViewModel = value;
            this.RaisePropertyChanged(nameof(DialogOpen));
            this.RaisePropertyChanged(nameof(CloseDialogOnClickAway));
            this.RaiseAndSetIfChanged(ref _dialogViewModel, value);
        }
    }

    public bool DialogOpen
    {
        get => DialogViewModel != null;
        set
        {
            if (!value)
            {
                DialogViewModel = null;
            }
        }
    }

    public bool CloseDialogOnClickAway
    {
        get => DialogViewModel?.CanClose ?? true;
    }
    #endregion

    #region IViewModelBase
    [JsonIgnore]
    public bool ShouldInit { get => false; }
    public Task Init() => throw new NotImplementedException();
    [JsonIgnore]
    public bool ShouldRefresh { get => false; }
    public Task Refresh() => throw new NotImplementedException();

    private bool _disposedValue;
    public void Dispose()
    {
        Dispose(!_disposedValue);
        _disposedValue = true;
        GC.SuppressFinalize(this);
    }
    protected void Dispose(bool disposing)
    {
        if (disposing)
        {

        }
    }
    #endregion
}
