using System.ComponentModel;
using System.Runtime.CompilerServices;
using CapstoneBackend.DAL;
using CapstoneDesktop.Utility;
using JetBrains.Annotations;

namespace CapstoneDesktop.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _password = string.Empty;

        private string _username = string.Empty;

        public MainWindowViewModel()
        {
            LoginCommand = new RelayCommand(login, canLogin);
            CreateAccountCommand = new RelayCommand(createAccount, canCreateAccount);
            UserNotExists = false;
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

        public bool UserNotExists { get; set; }

        public RelayCommand LoginCommand { get; set; }
        public RelayCommand CreateAccountCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private bool canLogin(object obj)
        {
            return !string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password);
        }

        private void login(object obj)
        {
            if (UserDAL.GetUser(_username, _password) != null)
            {
            }
            else
            {
                UserNotExists = true;
            }
        }

        private bool canCreateAccount(object obj)
        {
            return true;
        }

        private void createAccount(object obj)
        {
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}