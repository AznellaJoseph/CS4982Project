using System;
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
        private string _error = string.Empty;

        private bool _loginControlsVisible = true;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
        /// </summary>
        public MainWindowViewModel()
        {
            LoginCommand = ReactiveCommand.Create(this.login);
            OpenCreateAccountCommand = ReactiveCommand.Create(this.openCreateAccount);
            CancelCreateAccountCommand = ReactiveCommand.Create(this.cancelCreateAccount);
            SubmitAccountCommand = ReactiveCommand.Create(this.submitAccount);
        }

        public string? Username { get; set; }

        public string? Password { get; set; }

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
            set
            {
                this.RaiseAndSetIfChanged(ref _loginControlsVisible, value);
            }
        }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenCreateAccountCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CancelCreateAccountCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SubmitAccountCommand { get; set; }

        private void login()
        {
            var response = UserManager.GetUserByCredentials(Username ?? string.Empty, Password ?? string.Empty);
            if (string.IsNullOrEmpty(response.ErrorMessage))
                Console.WriteLine("YOU DID IT!");
            else
            {
                this.ErrorMessage = response.ErrorMessage;
            }
        }

        private void openCreateAccount()
        {
            this.LoginControlsVisible = false;
        }

        private void cancelCreateAccount()
        {
            this.LoginControlsVisible = true;
        }

        private void submitAccount()
        {
            var response = "";
            if (string.IsNullOrEmpty(response))
            {
                Console.WriteLine("YOU DID IT!");
            }
            else
            {
                this.ErrorMessage = response;
            }
        }
    }
}