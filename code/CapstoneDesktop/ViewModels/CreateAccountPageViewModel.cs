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
    public class CreateAccountPageViewModel : ReactiveViewModelBase
    {
        private readonly UserManager _userManager;
        private string _error = string.Empty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateAccountPageViewModel" /> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="screen">The screen.</param>
        public CreateAccountPageViewModel(UserManager manager, IScreen screen) : base(screen,
            Guid.NewGuid().ToString()[..5])
        {
            _userManager = manager;
            HostScreen = screen;
            CancelCreateAccountCommand =
                ReactiveCommand.CreateFromObservable(() => HostScreen.Router.NavigateBack.Execute());
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

        private IObservable<IRoutableViewModel> submitAccount()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(FirstName) ||
                string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(ConfirmedPassword))
            {
                ErrorMessage = Ui.ErrorMessages.InvalidFields;
                return Observable.Empty<IRoutableViewModel>();
            }

            if (Password == ConfirmedPassword)
            {
                var response = _userManager.RegisterUser(Username, Password,
                    FirstName, LastName);
                if (response.StatusCode == (uint) Ui.StatusCode.Success)
                    return HostScreen.Router.Navigate.Execute(
                        new LandingPageViewModel(new User {UserId = response.Data}, HostScreen));

                ErrorMessage = response.ErrorMessage ?? Ui.ErrorMessages.UnknownError;
                return Observable.Empty<IRoutableViewModel>();
            }

            ErrorMessage = Ui.ErrorMessages.PasswordsDoNotMatch;
            return Observable.Empty<IRoutableViewModel>();
        }
    }
}