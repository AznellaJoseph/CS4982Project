using System;
using System.Reactive;
using System.Reactive.Linq;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the MainWindow (Login)
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class LoginPageViewModel : ReactiveViewModelBase
    {
        private readonly UserManager _userManager;
        private string _error = string.Empty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoginPageViewModel" /> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="screen">The screen.</param>
        public LoginPageViewModel(UserManager manager, IScreen screen) : base(screen, Guid.NewGuid().ToString()[..5])
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

        private IObservable<IRoutableViewModel> login()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = Ui.ErrorMessages.InvalidFields;
                return Observable.Empty<IRoutableViewModel>();
            }

            var response = _userManager.GetUserByCredentials(Username, Password);
            if (string.IsNullOrEmpty(response.ErrorMessage) && response.Data is not null)
                return HostScreen.Router.Navigate.Execute(new LandingPageViewModel(response.Data, HostScreen));

            ErrorMessage = response.ErrorMessage ?? Ui.ErrorMessages.UnknownError;
            return Observable.Empty<IRoutableViewModel>();
        }
    }
}