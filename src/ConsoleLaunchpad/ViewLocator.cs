using System;
using System.Linq;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Threading;
using ConsoleLaunchpad.Imports;
using ConsoleLaunchpad.ViewModels;
using ConsoleLaunchpad.Views;

namespace ConsoleLaunchpad
{
    internal interface IBindableDataTemplate : IDataTemplate
    {
        public Control? BuildWithContext(IViewModelBase data);
    }

    // Singleton ViewLocator
    internal sealed partial class ViewLocator : IBindableDataTemplate
    {
        private ViewLocator()
        {
            if (Instance != null)
            {
                throw new InvalidOperationException("Only a single ViewLocator instance may be initialized.");
            }
        }

        internal static ViewLocator Instance { get; private set; } = new();

        [GeneratedRegex("ViewModel((?=s\\.)|$)")]

        private static partial Regex viewRegex();// = new("ViewModel((?=s\\.)|$)", RegexOptions.Compiled);

        static Type? GetType(object? data, out string name)
        {
            name = string.Empty;

            try
            {
                name = viewRegex().Replace(data?.GetType().FullName ?? string.Empty, "View");
                var type = TypeFromName(name);
                if (type == null && name.EndsWith("View"))
                {
                    name = name[0..^4];
                    type = TypeFromName(name);
                }
                return type;
            }
            catch (Exception e)
            {
                App.Logger.IfShouldLogCritical(() => e);
                return null;
            }

            static Type? TypeFromName(string name)
            {
                var type = AssemblyLoadContext.Default.Assemblies.Select(a => a.GetType(name)).SingleOrDefault(t => t != null);
                return type;
            }
        }

        public Control? Build(object? data)
        {
            var type = GetType(data, out var name);

            if (type != null)
            {
                try
                {
                    var control = (Control)Activator.CreateInstance(type)!;
                    return control;
                }
                catch (Exception e)
                {
                    return OnError(e, true);
                }
            }
            else
            {
                var e = new Exception($"Control '{name}' not found.");
                return OnError(e, true);
            }
        }

        public Control? BuildWithContext(IViewModelBase data)
        {
            var type = GetType(data, out var name);

            if (type != null)
            {
                try
                {
                    var control = (Control)Activator.CreateInstance(type)!;
                    if (data.ShouldInit)
                    {
                        Dispatcher.UIThread.InvokeAsync(async () =>
                        {
                            try
                            {
                                if (name.Contains("Profile")) throw new Exception("foobar");
                                await data.Init();
                            }
                            catch (Exception er)
                            {
                                OnError(er, false);
                            }
                        }).ConfigureAwait(false);
                    }
                    control.DataContext = data;
                    return control;
                }
                catch (Exception e)
                {
                    return OnError(e, true);
                }
            }
            else
            {
                var e = new Exception($"Control '{name}' not found.");
                return OnError(e, true);
            }
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }

        public Control? OnError(Exception er, bool fatal) {
            App.Logger.IfShouldLogCritical(() => er);
            App.OnError((er, fatal));
            return null;
        }
    }
}
