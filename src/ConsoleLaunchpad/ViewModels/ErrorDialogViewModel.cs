using System;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using ConsoleLaunchpad.Imports;
using ReactiveUI;
using static ConsoleLaunchpad.ViewModels.ErrorDialogViewModel;

namespace ConsoleLaunchpad.ViewModels
{
    public class ErrorDialogViewModel : ReactiveObject, IViewModelDialogBase<ErrorDialogViewModel, ErrorDialogViewModelEventType, ErrorDialogViewModelEventArgs>, IViewModelBase
    {
        readonly MainViewModel Parent;

#if DEBUG
        public ErrorDialogViewModel(MainViewModel parent)
        {
            Parent = parent;

            Exception exception = new("Example Error");
            Exception = exception;
            ErrorText = exception.ToString();
        }
#endif

        public ErrorDialogViewModel(MainViewModel parent, Exception exception, bool canContinue = false)
        {
            Parent = parent;

            Exception = exception;
            ErrorText = exception.GetType().FullName + ": " + exception.Message;

            CanContinue = canContinue;
        }
        internal ErrorDialogViewModel(MainViewModel parent, ErrorDialogViewModel viewModel, bool canContinue = false)
        {
            Parent = parent;

            ErrorText = viewModel.ErrorText;

            CanContinue = canContinue;
        }

        [JsonIgnore]
        public Exception? Exception { get; private init; }
        public string? ErrorText { get; private init; }

        public void Quit() => Submit(new(ErrorDialogViewModelEventType.Quit, this));

        public void Continue() => Submit(new(ErrorDialogViewModelEventType.Continue, this));

        public bool _canContinue = true;
        public bool CanContinue
        {
            get => _canContinue;
            set {
                this.RaisePropertyChanged(nameof(CanClose));
                this.RaiseAndSetIfChanged(ref _canContinue, value);
            }
        }
        public bool CanClose {
            get => _canContinue;
        }

        #region Dialog
        public enum ErrorDialogViewModelEventType
        {
            Quit,
            Continue
        }

        public class ErrorDialogViewModelEventArgs : ViewModelDialogBaseEventArgs<ErrorDialogViewModel, ErrorDialogViewModelEventType, ErrorDialogViewModelEventArgs>
        {
            public ErrorDialogViewModelEventArgs(ErrorDialogViewModelEventType type, ErrorDialogViewModel self) : base(type, self) { }
        }

        public event EventHandler<ErrorDialogViewModelEventArgs>? OnSubmit;

        public void Submit(ErrorDialogViewModelEventArgs e)
        {
            OnSubmit?.Invoke(this, e);
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
            if (disposing) { }
        }
        #endregion
    }
}
