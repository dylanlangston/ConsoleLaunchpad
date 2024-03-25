using System;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using ConsoleLaunchpad.Imports;
using ReactiveUI;
using static ConsoleLaunchpad.ViewModels.ProfilesDialogViewModel;

namespace ConsoleLaunchpad.ViewModels
{
    public class ProfilesDialogViewModel : ReactiveObject, IViewModelDialogBase<ProfilesDialogViewModel, ProfilesDialogViewModelEventType, ProfilesDialogViewModelEventArgs>, IViewModelBase
    {
        readonly ProfilesViewModel Parent;
        public ProfilesDialogViewModel(ProfilesViewModel parent)
        {
            Parent = parent;
        }

        public void Cancel() => Submit(new(ProfilesDialogViewModelEventType.Cancel, this));

        public void Ok() => Submit(new(ProfilesDialogViewModelEventType.Ok, this));

        public bool CanContinue
        {
            get => true;
        }

        #region Dialog
        public enum ProfilesDialogViewModelEventType
        {
            Cancel,
            Ok
        }

        public class ProfilesDialogViewModelEventArgs : ViewModelDialogBaseEventArgs<ProfilesDialogViewModel, ProfilesDialogViewModelEventType, ProfilesDialogViewModelEventArgs>
        {
            public ProfilesDialogViewModelEventArgs(ProfilesDialogViewModelEventType type, ProfilesDialogViewModel self) : base(type, self) { }
        }

        public event EventHandler<ProfilesDialogViewModelEventArgs>? OnSubmit;

        public void Submit(ProfilesDialogViewModelEventArgs e)
        {
            OnSubmit?.Invoke(this, e);
        }
        #endregion

        #region IViewModelBase
        [JsonIgnore]
        public bool ShouldInit { get => false; }
        public async Task Init() => throw new NotImplementedException();
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
