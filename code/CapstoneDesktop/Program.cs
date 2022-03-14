using System;
using Avalonia;
using Avalonia.ReactiveUI;
using CapstoneDesktop.ViewModels;
using CapstoneDesktop.Views;
using ReactiveUI;
using Splat;

namespace CapstoneDesktop
{
    /// <summary>
    ///     Program Class
    /// </summary>
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        /// <summary>
        ///     Builds the avalonia application.
        /// </summary>
        /// <returns> The app builder creating the application </returns>
        public static AppBuilder BuildAvaloniaApp()
        {
            Locator.CurrentMutable.Register(() => new TripOverviewPage(), typeof(IViewFor<TripOverviewPageViewModel>));
            Locator.CurrentMutable.Register(() => new LoginPage(), typeof(IViewFor<LoginPageViewModel>));
            Locator.CurrentMutable.Register(() => new CreateTripPage(), typeof(IViewFor<CreateTripPageViewModel>));
            Locator.CurrentMutable.Register(() => new CreateWaypointPage(),
                typeof(IViewFor<CreateWaypointPageViewModel>));
            Locator.CurrentMutable.Register(() => new CreateTransportationPage(),
                typeof(IViewFor<CreateTransportationPageViewModel>));
            Locator.CurrentMutable.Register(() => new CreateAccountPage(),
                typeof(IViewFor<CreateAccountPageViewModel>));
            Locator.CurrentMutable.Register(() => new CreateLodgingPage(), typeof(IViewFor<CreateLodgingPageViewModel>));
            Locator.CurrentMutable.Register(() => new LandingPage(), typeof(IViewFor<LandingPageViewModel>));
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
        }
    }
}