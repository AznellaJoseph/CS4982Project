using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CapstoneDesktop.ViewModels;
using CapstoneDesktop.Views;

namespace CapstoneDesktop
{
    /// <summary>
    ///     Application for Capstone Desktop
    /// </summary>
    /// <seealso cref="Avalonia.Application" />
    public class App : Application
    {
        /// <summary>
        ///     Initializes the application by loading XAML etc.
        /// </summary>
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        ///     Called when [framework initialization completed].
        /// </summary>
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel()
                };
            
            base.OnFrameworkInitializationCompleted();
        }
    }
}