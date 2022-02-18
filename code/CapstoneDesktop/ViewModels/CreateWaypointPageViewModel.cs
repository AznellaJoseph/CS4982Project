using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the CreateWaypoint Window
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class CreateWaypointPageViewModel : ViewModelBase, IRoutableViewModel
    {
        private readonly WaypointManager _waypointManager;
        private readonly Trip _trip;

        private string _error = string.Empty;
        
        public IScreen HostScreen { get; }
        public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        /// <summary>
        /// The create waypoint command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> CreateWaypointCommand { get; }

        /// <summary>
        /// The cancel create waypoint command.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelCreateWaypointCommand { get; }
        
        /// <summary>
        /// The error message.
        /// </summary>
        public string ErrorMessage
        {
            get => _error;
            set => this.RaiseAndSetIfChanged(ref _error, value);
        }

        /// <summary>
        /// The start date.
        /// </summary>
        public DateTimeOffset? StartDate { get; set; }

        /// <summary>
        /// The start time.
        /// </summary>
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// The end date.
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// The end time.
        /// </summary>
        public TimeSpan? EndTime { get; set; }

        /// <summary>
        /// The location.
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// The notes.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateWaypointPageViewModel" /> class.
        /// </summary>
        /// <param name="trip">the trip that the waypoint will be created for.</param>
        /// <param name="manager">The manager.</param>
        /// <param name="screen">The host screen</param>
        public CreateWaypointPageViewModel(Trip trip, WaypointManager manager, IScreen screen)
        {
            this._trip = trip;
            _waypointManager = manager;
            this.HostScreen = screen;
            CreateWaypointCommand = ReactiveCommand.CreateFromObservable(createWaypoint);
            CancelCreateWaypointCommand = ReactiveCommand.CreateFromObservable(this.HostScreen.Router.NavigateBack.Execute);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateWaypointPageViewModel" /> class.
        /// </summary>
        public CreateWaypointPageViewModel(Trip trip, IScreen screen) : this(trip, new WaypointManager(), screen)
        {
        }

        private IObservable<IRoutableViewModel> createWaypoint()
        {
            if (string.IsNullOrEmpty(Location))
            {
                ErrorMessage = "You must enter a location for the waypoint";
                return Observable.Empty<IRoutableViewModel>();
            }
            if (StartDate is null || StartTime is null)
            {
                ErrorMessage = "You must enter a start date and time for the waypoint";
                return Observable.Empty<IRoutableViewModel>();
            }

            var startDate = StartDate?.Date + StartTime;
            var endTime = EndDate is null || EndTime is null ? null : EndDate?.Date + EndTime;
            var resultResponse = _waypointManager.CreateWaypoint(this._trip.TripId, Location, startDate ?? DateTime.Now,
                    endTime, Notes);
            if (!string.IsNullOrEmpty(resultResponse.ErrorMessage))
            {
                    ErrorMessage = resultResponse.ErrorMessage;
                    return Observable.Empty<IRoutableViewModel>();
            }

            return this.HostScreen.Router.Navigate.Execute(new TripOverviewPageViewModel(this._trip, this.HostScreen));
        }
    }
}