using System;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _error = string.Empty;

        public MainWindowViewModel()
        {
            LoginCommand = ReactiveCommand.Create(this.login);
            CreateAccountCommand = ReactiveCommand.Create(this.createAccount);
        }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string ErrorMessage
        {
            get => _error;
            set
            {
                this.RaiseAndSetIfChanged(ref _error, value);
            }
        }

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateAccountCommand { get; set; }

        private void login()
        {
            var response = UserManager.GetUserByCredentials(this.Username ?? string.Empty, this.Password ?? string.Empty);
            if (string.IsNullOrEmpty(response.ErrorMessage))
            {
                Console.WriteLine("YOU DID IT!");
            }
            else
            {
                this.ErrorMessage = response.ErrorMessage;
            }
        }

        private void createAccount()
        {
        }
    }
}