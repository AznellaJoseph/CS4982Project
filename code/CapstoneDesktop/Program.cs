using System;
using Avalonia;
using Avalonia.ReactiveUI;
using CapstoneDesktop.ViewModels;
using CapstoneDesktop.Views;
using ReactiveUI;
using Splat;

namespace CapstoneDesktop
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        
        public static AppBuilder BuildAvaloniaApp()
        {
            Locator.CurrentMutable.Register(() => new LoginPage(), typeof(IViewFor<LoginPageViewModel>));
            Locator.CurrentMutable.Register(() => new CreateAccountPage(), typeof(IViewFor<CreateAccountPageViewModel>));
            Locator.CurrentMutable.Register(() => new LandingPage(), typeof(IViewFor<LandingPageViewModel>));
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
        }
    }
}