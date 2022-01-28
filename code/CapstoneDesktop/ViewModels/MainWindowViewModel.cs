using System;
using System.Threading.Tasks;
using Avalonia.Layout;
using CapstoneBackend.DAL;
using CapstoneBackend.Model;
using CapstoneDesktop.Utility;

namespace CapstoneDesktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _password = string.Empty;

        private string _username = string.Empty;

        public MainWindowViewModel()
        {
            LoginCommand = new RelayCommand(login, canLogin);
            CreateAccountCommand = new RelayCommand(createAccount, canCreateAccount);
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                LoginCommand.OnCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                LoginCommand.OnCanExecuteChanged();
            }
        }

        public string ErrorMessage { get; set; } = string.Empty;

        public RelayCommand LoginCommand { get; set; }
        public RelayCommand CreateAccountCommand { get; set; }

        private bool canLogin(object obj)
        {
            return !string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password);
        }

        private void login(object obj)
        {
            var loginTask = UserManager.LoginUser(this._username, this._password);
            if (string.IsNullOrEmpty(loginTask.Result.ErrorMessage))
            {
            }
            else
            {
                ErrorMessage = loginTask.Result.ErrorMessage;
            }
        }

        private bool canCreateAccount(object obj)
        {
            return true;
        }

        private void createAccount(object obj)
        {
        }
    }
}