using System;
using System.Reactive;
using System.Reactive.Linq;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    public class CreateAccountPageViewModel : ViewModelBase, IRoutableViewModel
    {
        private readonly UserManager _userManager;
        private string _error = string.Empty;
        
        public IScreen HostScreen { get; }
        public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
        
        public ReactiveCommand<Unit, Unit> CancelCreateAccountCommand { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> SubmitAccountCommand { get; }

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

        public CreateAccountPageViewModel(UserManager manager, IScreen screen)
        {
            this._userManager = manager;
            this.HostScreen = screen;
            this.CancelCreateAccountCommand = ReactiveCommand.CreateFromObservable(this.HostScreen.Router.NavigateBack.Execute);
            this.SubmitAccountCommand = ReactiveCommand.CreateFromObservable(this.submitAccount);
        }

        public CreateAccountPageViewModel(IScreen screen): this(new UserManager(), screen)
        {
        }

        private IObservable<IRoutableViewModel> submitAccount()
        {
           
            // TODO: remove later used for testing purposes.
            return this.HostScreen.Router.Navigate.Execute(new LandingPageViewModel(this.HostScreen));
            if (Password == ConfirmedPassword)
            {
                var response = this._userManager.RegisterUser(Username ?? string.Empty, Password ?? string.Empty, FirstName ?? string.Empty, LastName ?? string.Empty);
                if (response.StatusCode == 200)
                {
                    return this.HostScreen.Router.Navigate.Execute(new LandingPageViewModel(this.HostScreen));
                }
                else
                {
                    this.ErrorMessage = response.ErrorMessage ?? "Unknown Error.";
                    return Observable.Empty<IRoutableViewModel>();
                }    
            }
            else
            {
                this.ErrorMessage = "The given passwords must match.";
                return Observable.Empty<IRoutableViewModel>();
            }
            
        }
    }
}