namespace ConsoleLaunchpad.Imports
{
    public interface IViewModelBase : IDisposable
    {
        public bool ShouldInit { get; }
        public Task Init();
        public bool ShouldRefresh { get; }
        public Task Refresh();
    }

    public interface IViewModelBaseWithDialog : IViewModelBase
    {
        public IViewModelDialogBase? DialogViewModel { get; }
    }
}
