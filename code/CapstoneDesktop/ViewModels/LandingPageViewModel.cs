using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the Landing Page
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ReactiveViewModelBase" />
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    /// <seealso cref="ReactiveUI.IRoutableViewModel" />
    public class LandingPageViewModel : ReactiveViewModelBase
    {
        private readonly User _user;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LandingPageViewModel" /> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="screen">The screen.</param>
        public LandingPageViewModel(User user, IScreen screen) : base(screen,
            Guid.NewGuid().ToString()[..5])
        {
            _user = user;
            HostScreen = screen;
            TripViewModels = new ObservableCollection<TripViewModel>();
            CreateTripCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new CreateTripPageViewModel(_user, HostScreen)));
            LogoutCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new LoginPageViewModel(HostScreen)));
            loadTrips();
        }

        /// <summary>
        ///     The trip manager.
        /// </summary>
        public TripManager TripManager { get; set; } = new();

        /// <summary>
        ///     The create trip command
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> CreateTripCommand { get; }

        /// <summary>
        ///     The logout command
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> LogoutCommand { get; }

        /// <summary>
        ///     The trip view models
        /// </summary>
        public ObservableCollection<TripViewModel> TripViewModels { get; set; }

        private void loadTrips()
        {
            var response = TripManager.GetTripsByUser(_user.UserId);
            foreach (var trip in response.Data ?? new List<Trip>())
                TripViewModels.Add(new TripViewModel(trip, HostScreen));
        }
    }
}