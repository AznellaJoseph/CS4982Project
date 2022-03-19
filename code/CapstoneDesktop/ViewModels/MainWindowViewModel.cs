using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the MainWindow
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    /// <seealso cref="ReactiveUI.IScreen" />
    public class MainWindowViewModel : ViewModelBase, IScreen
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
        /// </summary>
        public MainWindowViewModel()
        {
            Router.Navigate.Execute(new LoginPageViewModel(this));
        }

        /// <summary>
        ///     The router
        /// </summary>
        public RoutingState Router { get; } = new();
    }
}