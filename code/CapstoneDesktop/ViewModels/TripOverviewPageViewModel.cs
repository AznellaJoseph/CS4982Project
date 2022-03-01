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
        private readonly EventManager _eventManager;
        
        private DateTime? _selectedDate;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TripOverviewPageViewModel" /> class.
        /// </summary>
        /// <param name="trip">The trip.</param>
        /// <param name="eventManager">The event manager.</param>
        /// <param name="screen">The screen.</param>

        public TripOverviewPageViewModel(Trip trip, EventManager eventManager, IScreen screen): base(screen, Guid.NewGuid().ToString()[..5])
        {
            Trip = trip;
            _eventManager = eventManager;
            HostScreen = screen;
            LogoutCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new LoginPageViewModel(HostScreen)));
            CreateWaypointCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new CreateWaypointPageViewModel(Trip, HostScreen)));
            BackCommand = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.NavigateBack.Execute());
            EventViewModels = new ObservableCollection<IEventViewModel>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TripOverviewPageViewModel" /> class.
        /// </summary>
        /// <param name="trip">The trip.</param>
        /// <param name="screen">The screen.</param>
        public TripOverviewPageViewModel(Trip trip, IScreen screen) : this(trip, new EventManager(), screen)
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
        ///     The waypoint viewmodels.
        /// </summary>
        public ObservableCollection<IEventViewModel> EventViewModels { get; }

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
            EventViewModels.Clear();
            if (SelectedDate is not null)
            {
                var response = _eventManager.GetEventsOnDate(Trip.TripId, (DateTime) SelectedDate);

                foreach (var aEvent in response.Data ?? new List<IEvent>())
                { 
                    IEventViewModel viewModel;
                    if (aEvent is Waypoint waypoint)
                        viewModel = new WaypointViewModel(waypoint, HostScreen);
                    else if (aEvent is Transportation transportation)
                        viewModel = new TransportationViewModel(transportation, HostScreen);
                    else
                        return;
                    
                    viewModel.RemoveEvent += (sender, e) =>
                    {
                        if (sender is not null)
                            this.EventViewModels.Remove((IEventViewModel)sender);
                    };
                    EventViewModels.Add(viewModel);
                }
                   
            }

        }
    }
}
