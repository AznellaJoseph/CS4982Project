using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using CapstoneDesktop.Utility;
using JetBrains.Annotations;

namespace CapstoneDesktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private string _username = string.Empty;
        private string _password = string.Empty;

        public string Username
        {
            get => _username;
            set => this._username = value;
        }

        public string Password
        {
            get => _password;
            set => this._password = value;
        }

        public RelayCommand LoginCommand { get; set; }
        public RelayCommand CreateAccountCommand { get; set; }

        public MainWindowViewModel()
        {
            this.LoginCommand = new RelayCommand(this.login, this.canLogin);
            this.CreateAccountCommand = new RelayCommand(this.createAccount, this.canCreateAccount);
        }

        private bool canLogin(object obj)
        {
            return true;
        }

        private void login(object obj)
        {

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