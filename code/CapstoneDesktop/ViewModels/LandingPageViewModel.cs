using System;
using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    public class LandingPageViewModel : ViewModelBase, IRoutableViewModel
    {
        public IScreen HostScreen { get; }
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
        
        public ReactiveCommand<Unit, Unit> CreateTripCommand { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> LogoutCommand { get; }
        
        public ObservableCollection<TripViewModel> TripViewModels { get; set; }
        
        
        public LandingPageViewModel(IScreen screen)
        {
            this.HostScreen = screen;
            this.TripViewModels = new ObservableCollection<TripViewModel>();
            this.CreateTripCommand = ReactiveCommand.Create(() =>
            {
                this.TripViewModels?.Add(new(new()
                {
                    Name = $"Test {(new Random()).Next(100)}",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now
                }));
            });
            this.LogoutCommand = ReactiveCommand.CreateFromObservable(() => this.HostScreen.Router.Navigate.Execute(new LoginPageViewModel(this.HostScreen)));
        }
    }
}