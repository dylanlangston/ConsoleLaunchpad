using System;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Avalonia.Controls;
using ConsoleLaunchpad.Imports;
using ReactiveUI;
using static ConsoleLaunchpad.ViewModels.ProfilesDialogViewModel;

namespace ConsoleLaunchpad.ViewModels
{
    public class ProfilesViewModel : ReactiveObject, IViewModelBaseWithDialog
    {
        public void OpenDialog()
        {
            var dialogViewModel = new ProfilesDialogViewModel(this);
            dialogViewModel.OnSubmit += (s, e) =>
            {
                switch (e.Type)
                {
                    default:
                        throw new NotImplementedException();
                }
            };
            DialogViewModel = dialogViewModel;
        }

        #region Dialog
        public Control? DialogView
        {
            get
            {
                var view = DialogViewModel != null ? App.ViewLocator?.BuildWithContext(DialogViewModel) : null;
                return view;
            }
        }

        IViewModelDialogBase? _dialogViewModel;
        public IViewModelDialogBase? DialogViewModel
        {
            get => _dialogViewModel;
            set
            {
                _dialogViewModel = value;
                this.RaisePropertyChanged(nameof(DialogOpen));
                this.RaiseAndSetIfChanged(ref _dialogViewModel, value);
                this.RaisePropertyChanged(nameof(DialogView));
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

        bool _closeDialogOnClickAway = true;
        public bool CloseDialogOnClickAway
        {
            get => _closeDialogOnClickAway;
            set => this.RaiseAndSetIfChanged(ref _closeDialogOnClickAway, value);
        }
        #endregion

        #region IViewModelBase
        [JsonIgnore]
        public bool ShouldInit { get => false; }
        public Task Init() => throw new NotImplementedException();
        [JsonIgnore]
        public bool ShouldRefresh { get => false; }
        public async Task Refresh() => throw new NotImplementedException();

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
}
