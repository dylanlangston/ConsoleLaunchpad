using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ConsoleLaunchpad.Imports;
using ConsoleLaunchpad.ViewModels;
using ConsoleLaunchpad.Views;
using Material.Colors;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;

namespace ConsoleLaunchpad;

public partial class App : Application
{
    public static readonly IViewLocator<Control> ViewLocator = MEF.GetExport<IViewLocator<Control>>() ?? new NullViewLocator<Control>();
    public static readonly IConfig Config = MEF.GetExport<IConfig>() ?? new NullConfig();
    public static readonly ISettings Settings = MEF.GetExport<ISettings>() ?? new NullSettings();
    public static readonly ILogger Logger = MEF.GetExport<ILogger>() ?? new NullLogger();
    public static void OnError(AppErrorEventArgs e) => ((App?)App.Current)?.ErrorOccured?.Invoke(Current, e);
    
    event EventHandler<AppErrorEventArgs>? ErrorOccured;

    public override void Initialize()
    {
        App.Logger.IfShouldLogInformation(() => "App Framework Initialization Started");

        AvaloniaXamlLoader.Load(this);
    }

    public IViewModelBase GetVM() {
        var VM = new MainViewModel();
        ErrorOccured += (s, e) => {
            VM.OpenErrorDialog(e.Exception, e.Fatal);
        };
        return VM;
    }

    const double DefaultWidth = 750;
    const double DefaultHeight = 650;
    public override void OnFrameworkInitializationCompleted()
    {
        // Load Plugin DLLs
        // MEF.AddAssemblies(App.Settings.Plugins, (er) =>
        // {
        //     App.Logger.IfShouldLogCritical(() => er);
        //     Program.OnError(er);
        // });

        SetupViewModel(
            GetVM(),
            WindowState.Normal,
            DefaultWidth,
            DefaultHeight,
            (desktop) => (s, e) =>
            {
                if (desktop.MainWindow!.WindowState == WindowState.Minimized)
                {
                    
                }
                else
                {
                    
                }
            });

        base.OnFrameworkInitializationCompleted();

        var themeBootstrap = this.LocateMaterialTheme<MaterialThemeBase>();
        themeBootstrap.CurrentTheme = LoadOrCreateDefaultTheme();

        // themeBootstrap.CurrentThemeChanged.Subscribe(newTheme => {

        // });

        App.Logger.IfShouldLogInformation(() => "App Framework Initialization Completed");
    }

    void SetupViewModel(
        IViewModelBase viewModel,
        WindowState windowState,
        double width,
        double height,
        Func<IClassicDesktopStyleApplicationLifetime, EventHandler<WindowResizedEventArgs>> resized)
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel,
                WindowState = windowState,
            };
            desktop.MainWindow.Width = Math.Min(desktop.MainWindow.Screens.Primary?.Bounds.Width ?? DefaultWidth, width);
            desktop.MainWindow.Height = Math.Min(desktop.MainWindow.Screens.Primary?.Bounds.Height ?? DefaultHeight, height);
            desktop.MainWindow.Resized += resized(desktop);

            desktop.Exit += (s, e) => Shutdown(viewModel);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = viewModel
            };
            singleViewPlatform.MainView.Unloaded += (s, e) => Shutdown(viewModel);
        }
    }

    ITheme LoadOrCreateDefaultTheme()
    {
        var primary = PrimaryColor.Blue;
        var primaryColor = SwatchHelper.Lookup[(MaterialColor)primary];

        var secondary = SecondaryColor.Lime;
        var secondaryColor = SwatchHelper.Lookup[(MaterialColor)secondary];

        return Theme.Create(Theme.Light, primaryColor, secondaryColor);
    }

    void Shutdown(IViewModelBase viewModel)
    {
        viewModel.Dispose();
        Settings.Dispose();
    }
}

public class AppErrorEventArgs : EventArgs {
    public Exception Exception;
    public bool Fatal;
    public AppErrorEventArgs(Exception exception, bool fatal) : base() {
        Exception = exception;
        Fatal = fatal;
    }

    public static implicit operator AppErrorEventArgs((Exception err, bool fatal) args) => new(args.err, args.fatal);
    public static implicit operator (Exception err, bool fatal)(AppErrorEventArgs args) => (args.Exception, args.Fatal);
}