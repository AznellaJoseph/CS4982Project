using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the Trip Overview Page
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    /// <seealso cref="ReactiveUI.IRoutableViewModel" />
    public class TripOverviewPageViewModel : ReactiveViewModelBase
    {
        private readonly WaypointManager _waypointManager;
        private DateTime? _selectedDate;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TripOverviewPageViewModel" /> class.
        /// </summary>
        /// <param name="trip">The trip.</param>
        /// <param name="waypointManager">The waypoint manager.</param>
        /// <param name="screen">The screen.</param>
        public TripOverviewPageViewModel(Trip trip, WaypointManager waypointManager, IScreen screen) : base(screen,
            Guid.NewGuid().ToString()[..5])
        {
            Trip = trip;
            _waypointManager = waypointManager;
            HostScreen = screen;
            LogoutCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new LoginPageViewModel(HostScreen)));
            CreateWaypointCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new CreateWaypointPageViewModel(Trip, HostScreen)));
            CreateTransportationCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new CreateWaypointPageViewModel(Trip, HostScreen)));
            BackCommand = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.NavigateBack.Execute());
            WaypointViewModels = new ObservableCollection<WaypointViewModel>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TripOverviewPageViewModel" /> class.
        /// </summary>
        /// <param name="trip">The trip.</param>
        /// <param name="screen">The screen.</param>
        public TripOverviewPageViewModel(Trip trip, IScreen screen) : this(trip, new WaypointManager(), screen)
        {
        }

        /// <summary>
        ///     The logout command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> LogoutCommand { get; }

        /// <summary>
        ///     The back command.
        /// </summary>
        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        /// <summary>
        ///     The create waypoint command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> CreateWaypointCommand { get; }

        /// <summary>
        ///   The create transportation command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> CreateTransportationCommand { get; }

        /// <summary>
        ///     The waypoint viewmodels.
        /// </summary>
        public ObservableCollection<WaypointViewModel> WaypointViewModels { get; }

        /// <summary>
        ///     The trip.
        /// </summary>
        public Trip Trip { get; }

        /// <summary>
        ///     The selected date.
        /// </summary>
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedDate, value, nameof(SelectedDate));
                updateWaypoints();
            }
        }

        private void updateWaypoints()
        {
            WaypointViewModels.Clear();
            if (SelectedDate is null) return;
            var response = _waypointManager.GetWaypointsOnDate(Trip.TripId, (DateTime) SelectedDate);
            foreach (var waypoint in response.Data ?? new List<Waypoint>())
                WaypointViewModels.Add(new WaypointViewModel(waypoint, HostScreen));
        }
    }
}