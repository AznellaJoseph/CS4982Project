using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the MainWindow (Login)
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class LoginPageViewModel : ViewModelBase, IRoutableViewModel
    {
        private readonly UserManager _userManager;
        private string _error = string.Empty;

        public IScreen HostScreen { get; }
        public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ReactiveCommand<Unit, IRoutableViewModel> LoginCommand { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> OpenCreateAccountCommand { get; set; }

        public string? Username { get; set; }
        public string? Password { get; set; }
        public string ErrorMessage
        {
            get => _error;
            set => this.RaiseAndSetIfChanged(ref _error, value);
        }
        
        public LoginPageViewModel(UserManager manager, IScreen screen)
        {
            this._userManager = manager;
            this.HostScreen = screen;
            this.LoginCommand = ReactiveCommand.CreateFromObservable(this.login);
            this.OpenCreateAccountCommand = ReactiveCommand.CreateFromObservable(() => this.HostScreen.Router.Navigate.Execute(new CreateAccountPageViewModel(this.HostScreen)));
        }
        
        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
        /// </summary>
        public LoginPageViewModel(IScreen screen) : this(new UserManager(), screen)
        {
        }

        private IObservable<IRoutableViewModel> login()
        {
            var response = this._userManager.GetUserByCredentials(Username ?? string.Empty, Password ?? string.Empty);
            if (response.StatusCode == 200U && response.Data is not null)
            {
                return this.HostScreen.Router.Navigate.Execute(new LandingPageViewModel(response.Data, new TripManager(), this.HostScreen));
            }
            this.ErrorMessage = response.ErrorMessage ?? string.Empty; 
            return Observable.Empty<IRoutableViewModel>();
        }
    }
}