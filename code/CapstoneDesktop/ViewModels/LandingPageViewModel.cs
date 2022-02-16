using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    public class LandingPageViewModel : ViewModelBase, IRoutableViewModel
    {
        private readonly TripManager _tripManager;
        private readonly User _user;
        
        public IScreen HostScreen { get; }
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
        
        public ReactiveCommand<Unit, Unit> CreateTripCommand { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> LogoutCommand { get; }
        
        public ObservableCollection<TripViewModel> TripViewModels { get; set; }
        
        
        public LandingPageViewModel(User user, TripManager tripManager, IScreen screen)
        {
            this._user = user;
            this._tripManager = tripManager;
            this.HostScreen = screen;
            this.TripViewModels = new ObservableCollection<TripViewModel>();
            this.CreateTripCommand = ReactiveCommand.Create(this.createTrip);
            this.LogoutCommand = ReactiveCommand.CreateFromObservable(() => this.HostScreen.Router.Navigate.Execute(new LoginPageViewModel(this.HostScreen)));
            this.loadTrips();
        }

        public LandingPageViewModel(User user, IScreen screen) : this(user, new TripManager(), screen)
        {
        }

        private void loadTrips()
        {
            var response = this._tripManager.GetTripsByUser(this._user.UserId);
            foreach (var trip in response.Data ?? new List<Trip>())
            {
                this.TripViewModels.Add(new TripViewModel(trip, this.HostScreen));
            }
        }

        private void createTrip()
        {
            
        }
    }
}