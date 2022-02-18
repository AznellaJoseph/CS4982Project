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
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    /// <seealso cref="ReactiveUI.IRoutableViewModel" />
    public class LandingPageViewModel : ViewModelBase, IRoutableViewModel
    {
        private readonly TripManager _tripManager;
        private readonly User _user;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LandingPageViewModel" /> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="tripManager">The trip manager.</param>
        /// <param name="screen">The screen.</param>
        public LandingPageViewModel(User user, TripManager tripManager, IScreen screen)
        {
            _user = user;
            _tripManager = tripManager;
            HostScreen = screen;
            TripViewModels = new ObservableCollection<TripViewModel>();
            CreateTripCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new CreateTripPageViewModel(_user, HostScreen)));
            LogoutCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new LoginPageViewModel(HostScreen)));
            loadTrips();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LandingPageViewModel" /> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="screen">The screen.</param>
        public LandingPageViewModel(User user, IScreen screen) : this(user, new TripManager(), screen)
        {
        }

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

        /// <summary>
        ///     The host screen
        /// </summary>
        public IScreen HostScreen { get; }

        /// <summary>
        ///     The url path segment
        /// </summary>
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        private void loadTrips()
        {
            var response = _tripManager.GetTripsByUser(_user.UserId);
            foreach (var trip in response.Data ?? new List<Trip>())
                TripViewModels.Add(new TripViewModel(trip, HostScreen));
        }
    }
}