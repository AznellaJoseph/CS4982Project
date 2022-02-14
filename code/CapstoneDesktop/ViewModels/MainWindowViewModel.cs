using System;
using System.Diagnostics;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the MainWindow (Login)
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly UserManager _userManager;

        private string _error = string.Empty;

        private bool _loginControlsVisible = true;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public MainWindowViewModel(UserManager manager)
        {
            _userManager = manager;
            LoginCommand = ReactiveCommand.Create(login);
            OpenCreateAccountCommand = ReactiveCommand.Create(openCreateAccount);
            CancelCreateAccountCommand = ReactiveCommand.Create(cancelCreateAccount);
            SubmitAccountCommand = ReactiveCommand.Create(submitAccount);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
        /// </summary>
        public MainWindowViewModel() : this(new UserManager())
        {
        }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? ConfirmedPassword { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string ErrorMessage
        {
            get => _error;
            set => this.RaiseAndSetIfChanged(ref _error, value);
        }

        public bool LoginControlsVisible
        {
            get => _loginControlsVisible;
            set => this.RaiseAndSetIfChanged(ref _loginControlsVisible, value);
        }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenCreateAccountCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CancelCreateAccountCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SubmitAccountCommand { get; set; }

        private void login()
        {
            var response = _userManager.GetUserByCredentials(Username ?? string.Empty, Password ?? string.Empty);
            if (response.Data is not null)
                Console.WriteLine("YOU DID IT!");
            else
                ErrorMessage = response.ErrorMessage ?? string.Empty;
        }

        private void openCreateAccount()
        {
            LoginControlsVisible = false;
        }

        private void cancelCreateAccount()
        {
            LoginControlsVisible = true;
        }

        private void submitAccount()
        {
            if (Password == ConfirmedPassword)
            {
                var response = _userManager.RegisterUser(Username ?? string.Empty, Password ?? string.Empty,
                    FirstName ?? string.Empty, LastName ?? string.Empty);

                if (response.StatusCode == 200)
                    Debug.WriteLine("Successful Account Creation");
                else
                    ErrorMessage = response.ErrorMessage ?? "Unknown Error.";
            }
            else
            {
                ErrorMessage = "The given passwords must match.";
            }
        }
    }
}