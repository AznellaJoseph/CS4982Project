using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IScreen
    {
        
        public RoutingState Router { get; } = new();

        public MainWindowViewModel()
        {
            this.Router.Navigate.Execute(new LoginPageViewModel(this));
        }
        
    }
}