using System;
using System.Reactive;
using System.Reactive.Linq;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for Create Account Page
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    /// <seealso cref="ReactiveUI.IRoutableViewModel" />
    public class CreateAccountPageViewModel : ViewModelBase, IRoutableViewModel
    {
        private readonly UserManager _userManager;
        private string _error = string.Empty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateAccountPageViewModel" /> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="screen">The screen.</param>
        public CreateAccountPageViewModel(UserManager manager, IScreen screen)
        {
            _userManager = manager;
            HostScreen = screen;
            CancelCreateAccountCommand =
                ReactiveCommand.CreateFromObservable(() => this.HostScreen.Router.NavigateBack.Execute());
            SubmitAccountCommand = ReactiveCommand.CreateFromObservable(submitAccount);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateAccountPageViewModel" /> class.
        /// </summary>
        /// <param name="screen">The screen.</param>
        public CreateAccountPageViewModel(IScreen screen) : this(new UserManager(), screen)
        {
        }

        /// <summary>
        ///     The cancel create account command
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelCreateAccountCommand { get; }

        /// <summary>
        ///     The submit account command
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> SubmitAccountCommand { get; }

        /// <summary>
        ///     The username
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        ///     The password
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        ///     The confirmed password
        /// </summary>
        public string? ConfirmedPassword { get; set; }

        /// <summary>
        ///     The first name
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        ///     The last name
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        ///     The error message
        /// </summary>
        public string ErrorMessage
        {
            get => _error;
            set => this.RaiseAndSetIfChanged(ref _error, value);
        }

        /// <summary>
        ///     The host screen.
        /// </summary>
        public IScreen HostScreen { get; }

        /// <summary>
        ///     The url path segment.
        /// </summary>
        public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        private IObservable<IRoutableViewModel> submitAccount()
        {
            if (Password == ConfirmedPassword)
            {
                var response = _userManager.RegisterUser(Username ?? string.Empty, Password ?? string.Empty,
                    FirstName ?? string.Empty, LastName ?? string.Empty);
                if (response.StatusCode == 200U)
                {
                    return HostScreen.Router.Navigate.Execute(
                        new LandingPageViewModel(new User { UserId = response.Data }, HostScreen));
                }

                ErrorMessage = response.ErrorMessage ?? ErrorMessages.UnknownError;
                return Observable.Empty<IRoutableViewModel>();
            }

            ErrorMessage = ErrorMessages.PasswordsDoNotMatch;
            return Observable.Empty<IRoutableViewModel>();
        }
    }
}