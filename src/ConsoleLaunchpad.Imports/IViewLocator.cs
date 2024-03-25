namespace ConsoleLaunchpad.Imports
{
    public interface IViewLocator<T> : IImport where T : class
    {
        public T? Build(object? data);
        public T? BuildWithContext(IViewModelBase data);
    }
    public sealed class NullViewLocator<T> : IViewLocator<T> where T : class
    {
        public T? Build(object? data)
        {
            return default(T);
        }
        public T? BuildWithContext(IViewModelBase data)
        {
            return default(T);
        }
    }
}
