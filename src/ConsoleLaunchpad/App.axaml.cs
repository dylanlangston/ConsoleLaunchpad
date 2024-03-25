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

    public override void Initialize()
    {
        App.Logger.IfShouldLogInformation(() => "App Framework Initialization Started");

        AvaloniaXamlLoader.Load(this);
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
            new MainViewModel(),
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
        ViewModelBase viewModel,
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
        }
    }

    ITheme LoadOrCreateDefaultTheme()
    {
        var primary = PrimaryColor.Cyan;
        var primaryColor = SwatchHelper.Lookup[(MaterialColor)primary];

        var secondary = SecondaryColor.Orange;
        var secondaryColor = SwatchHelper.Lookup[(MaterialColor)secondary];

        return Theme.Create(Theme.Dark, primaryColor, secondaryColor);
    }

    void Shutdown(ViewModelBase viewModel)
    {
        viewModel.Dispose();
        Settings.Dispose();
    }
}