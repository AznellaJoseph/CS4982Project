using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the Create Waypoint Page
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class CreateWaypointPageViewModel : ReactiveViewModelBase
    {
        private readonly Trip _trip;

        private string _error = string.Empty;

        private string _location = string.Empty;

        private IEnumerable<string> _predictions = new List<string>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateWaypointPageViewModel" /> class.
        /// </summary>
        /// <param name="trip">the trip that the waypoint is being created for.</param>
        /// <param name="screen">The screen</param>
        public CreateWaypointPageViewModel(Trip trip, IScreen screen) : base(screen,
            Guid.NewGuid().ToString()[..5])
        {
            _trip = trip;
            HostScreen = screen;
            CreateWaypointCommand = ReactiveCommand.CreateFromObservable(CreateWaypoint);
            CancelCreateWaypointCommand =
                ReactiveCommand.CreateFromObservable(() => HostScreen.Router.NavigateBack.Execute());
        }

        /// <summary>
        ///     The waypoint manager.
        /// </summary>
        public WaypointManager WaypointManager { get; set; } = new();

        /// <summary>
        ///     The validation manager.
        /// </summary>
        public ValidationManager ValidationManager { get; set; } = new();

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
        ///     The location.
        /// </summary>
        public string Location
        {
            get => _location;
            set
            {
                this.RaiseAndSetIfChanged(ref _location, value);
                UpdateAutoCompleteResultsAsync();
            }
        }

        /// <summary>
        ///     List of autocomplete results shown in the dropdown.
        /// </summary>
        public IEnumerable<string> AutocompletePredictions
        {
            get => _predictions;
            set => this.RaiseAndSetIfChanged(ref _predictions, value);
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
        ///     The notes.
        /// </summary>
        public string? Notes { get; set; }

        private async void UpdateAutoCompleteResultsAsync()
        {
            AutocompletePredictions = await GooglePlacesService.Autocomplete(Location);
        }

        private IObservable<IRoutableViewModel> CreateWaypoint()
        {
            if (string.IsNullOrEmpty(Location))
            {
                ErrorMessage = Ui.ErrorMessages.EmptyLocation;
                return Observable.Empty<IRoutableViewModel>();
            }

            if (StartDate is null || StartTime is null)
            {
                ErrorMessage = Ui.ErrorMessages.InvalidEventDate;
                return Observable.Empty<IRoutableViewModel>();
            }

            var validLocationResponse = ValidationManager.DetermineIfValidLocation(Location);

            if (!string.IsNullOrEmpty(validLocationResponse.ErrorMessage))
            {
                ErrorMessage = validLocationResponse.ErrorMessage;
                return Observable.Empty<IRoutableViewModel>();
            }

            var startDate = StartDate.Value.Date + StartTime.Value;

            var endDate = EndDate is null || EndTime is null ? _trip.EndDate : EndDate.Value.Date + EndTime.Value;

            var validDatesResponse = ValidationManager.DetermineIfValidEventDates(_trip.TripId, startDate, endDate);

            if (!string.IsNullOrEmpty(validDatesResponse.ErrorMessage))
            {
                ErrorMessage = validDatesResponse.ErrorMessage;
                return Observable.Empty<IRoutableViewModel>();
            }

            var clashingEventResponse =
                ValidationManager.FindClashingEvent(_trip.TripId, startDate, endDate);

            if (!string.IsNullOrEmpty(clashingEventResponse.ErrorMessage))
            {
                ErrorMessage = clashingEventResponse.ErrorMessage;
                return Observable.Empty<IRoutableViewModel>();
            }

            var resultResponse = WaypointManager.CreateWaypoint(_trip.TripId, Location, startDate,
                endDate, Notes);
            if (string.IsNullOrEmpty(resultResponse.ErrorMessage))
                return HostScreen.Router.Navigate.Execute(new TripOverviewPageViewModel(_trip, HostScreen,
                    new LodgingManager()));

            ErrorMessage = resultResponse.ErrorMessage;
            return Observable.Empty<IRoutableViewModel>();
        }
    }
}