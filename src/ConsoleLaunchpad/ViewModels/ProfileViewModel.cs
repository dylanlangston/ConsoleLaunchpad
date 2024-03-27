using System;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Avalonia.Controls;
using ConsoleLaunchpad.Imports;
using ReactiveUI;

namespace ConsoleLaunchpad.ViewModels
{
    public class ProfileViewModel : ReactiveObject, IViewModelBase
    {
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
}
