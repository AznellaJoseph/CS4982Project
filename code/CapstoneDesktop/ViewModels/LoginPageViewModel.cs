using System;
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

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoginPageViewModel" /> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="screen">The screen.</param>
        public LoginPageViewModel(UserManager manager, IScreen screen)
        {
            _userManager = manager;
            HostScreen = screen;
            LoginCommand = ReactiveCommand.CreateFromObservable(login);
            OpenCreateAccountCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new CreateAccountPageViewModel(HostScreen)));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
        /// </summary>
        /// <param name="screen">The screen.</param>
        public LoginPageViewModel(IScreen screen) : this(new UserManager(), screen)
        {
        }

        /// <summary>
        ///     The login command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> LoginCommand { get; }

        /// <summary>
        ///     The open create account command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> OpenCreateAccountCommand { get; set; }

        /// <summary>
        ///     The username.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        ///     The password.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        ///     The error message.
        /// </summary>
        public string ErrorMessage
        {
            get => _error;
            set => this.RaiseAndSetIfChanged(ref _error, value);
        }

        /// <summary>
        ///     The host screen
        /// </summary>
        public IScreen HostScreen { get; }

        /// <summary>
        ///     The url path segment.
        /// </summary>
        public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        private IObservable<IRoutableViewModel> login()
        {
            var response = _userManager.GetUserByCredentials(Username ?? string.Empty, Password ?? string.Empty);
            if (response.StatusCode == 200U && response.Data is not null)
                return HostScreen.Router.Navigate.Execute(new LandingPageViewModel(response.Data, HostScreen));
            ErrorMessage = response.ErrorMessage ?? string.Empty;
            return Observable.Empty<IRoutableViewModel>();
        }
    }
}