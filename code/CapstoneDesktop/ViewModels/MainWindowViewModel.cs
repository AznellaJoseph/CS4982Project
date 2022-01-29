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

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
        /// </summary>
        public MainWindowViewModel()
        {
            LoginCommand = ReactiveCommand.Create(login);
            CreateAccountCommand = ReactiveCommand.Create(createAccount);
        }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string ErrorMessage
        {
            get => _error;
            set => this.RaiseAndSetIfChanged(ref _error, value);
        }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateAccountCommand { get; set; }

        private void login()
        {
            var response = UserManager.GetUserByCredentials(Username ?? string.Empty, Password ?? string.Empty);
            if (string.IsNullOrEmpty(response.ErrorMessage))
                Console.WriteLine("YOU DID IT!");
            else
                ErrorMessage = response.ErrorMessage;
        }

        private void createAccount()
        {
        }
    }
}