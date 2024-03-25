using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ConsoleLaunchpad.Imports;
using ReactiveUI;

namespace ConsoleLaunchpad.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject, IViewModelBase, IDisposable
    {
        [JsonIgnore]
        public virtual bool ShouldInit { get => false; }
        public virtual Task Init() => throw new NotImplementedException();
        [JsonIgnore]
        public virtual bool ShouldRefresh { get => false; }
        public virtual Task Refresh() => throw new NotImplementedException();

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public new IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changing => base.Changing;
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public new IObservable<IReactivePropertyChangedEventArgs<IReactiveObject>> Changed => base.Changed;
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public new IObservable<Exception> ThrownExceptions => base.ThrownExceptions;

        private bool _disposedValue;
        public void Dispose()
        {
            Dispose(!_disposedValue);
            _disposedValue = true;
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing) { }
    }

    public abstract class ViewModelBaseWithDialog : ViewModelBase, IViewModelBaseWithDialog
    {
        public virtual IViewModelDialogBase? DialogViewModel { get => null; }
    }
}
