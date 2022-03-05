using System;
using System.Reactive;
using System.Reactive.Linq;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the CreateWaypoint Window
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class CreateWaypointPageViewModel : ReactiveViewModelBase
    {
        private readonly EventManager _eventManager = new();
        private readonly Trip _trip;
        private readonly WaypointManager _waypointManager;

        private string _error = string.Empty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateWaypointPageViewModel" /> class.
        /// </summary>
        /// <param name="trip">the trip that the waypoint will be created for.</param>
        /// <param name="manager">The manager.</param>
        /// <param name="screen">The host screen</param>
        public CreateWaypointPageViewModel(Trip trip, WaypointManager manager, IScreen screen) : base(screen,
            Guid.NewGuid().ToString()[..5])
        {
            _trip = trip;
            _waypointManager = manager;
            HostScreen = screen;
            CreateWaypointCommand = ReactiveCommand.CreateFromObservable(createWaypoint);
            CancelCreateWaypointCommand =
                ReactiveCommand.CreateFromObservable(() => HostScreen.Router.NavigateBack.Execute());
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateWaypointPageViewModel" /> class.
        /// </summary>
        /// <param name="trip">The trip.</param>
        /// <param name="screen">The screen.</param>
        public CreateWaypointPageViewModel(Trip trip, IScreen screen) : this(trip, new WaypointManager(), screen)
        {
        }

        /// <summary>
        ///     The create waypoint command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> CreateWaypointCommand { get; }

        /// <summary>
        ///     The cancel create waypoint command.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelCreateWaypointCommand { get; }

        /// <summary>
        ///     The error message.
        /// </summary>
        public string ErrorMessage
        {
            get => _error;
            set => this.RaiseAndSetIfChanged(ref _error, value);
        }

        /// <summary>
        ///     The start date.
        /// </summary>
        public DateTimeOffset? StartDate { get; set; }

        /// <summary>
        ///     The start time.
        /// </summary>
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        ///     The end date.
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        ///     The end time.
        /// </summary>
        public TimeSpan? EndTime { get; set; }

        /// <summary>
        ///     The location.
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        ///     The notes.
        /// </summary>
        public string? Notes { get; set; }

        private IObservable<IRoutableViewModel> createWaypoint()
        {
            if (string.IsNullOrEmpty(Location))
            {
                ErrorMessage = Ui.ErrorMessages.EmptyWaypointLocation;
                return Observable.Empty<IRoutableViewModel>();
            }

            if (StartDate is null || StartTime is null)
            {
                ErrorMessage = Ui.ErrorMessages.InvalidEventDate;
                return Observable.Empty<IRoutableViewModel>();
            }

            var startDate = StartDate.Value.Date + StartTime.Value;

            var endDate = EndDate is null || EndTime is null ? _trip.EndDate : EndDate.Value.Date + EndTime.Value;

            if (startDate.CompareTo(_trip.StartDate) < 0)
            {
                ErrorMessage = Ui.ErrorMessages.EventStartDateBeforeTripStartDate + _trip.StartDate.ToShortDateString();
                return Observable.Empty<IRoutableViewModel>();
            }

            if (startDate.CompareTo(_trip.EndDate) > 0)
            {
                ErrorMessage = Ui.ErrorMessages.EventStartDateAfterTripEndDate + _trip.EndDate.ToShortDateString();
                return Observable.Empty<IRoutableViewModel>();
            }

            if (endDate.CompareTo(_trip.StartDate) < 0)
            {
                ErrorMessage = Ui.ErrorMessages.EventEndDateBeforeTripStartDate + _trip.StartDate.ToShortDateString();
                return Observable.Empty<IRoutableViewModel>();
            }

            if (endDate.CompareTo(_trip.EndDate) > 0)
            {
                ErrorMessage = Ui.ErrorMessages.EventEndDateAfterTripEndDate + _trip.EndDate.ToShortDateString();
                return Observable.Empty<IRoutableViewModel>();
            }

            var clashingEvent = _eventManager.FindClashingEvent(_trip.TripId, startDate, endDate).Data;
            if (clashingEvent is not null)
            {
                ErrorMessage =
                    $"{Ui.ErrorMessages.ClashingEventDates} {clashingEvent.StartDate} to {clashingEvent.EndDate}.";
                return Observable.Empty<IRoutableViewModel>();
            }


            var resultResponse = _waypointManager.CreateWaypoint(_trip.TripId, Location, startDate,
                endDate, Notes);
            if (string.IsNullOrEmpty(resultResponse.ErrorMessage))
                return HostScreen.Router.Navigate.Execute(new TripOverviewPageViewModel(_trip, HostScreen));

            ErrorMessage = resultResponse.ErrorMessage;
            return Observable.Empty<IRoutableViewModel>();
        }
    }
}