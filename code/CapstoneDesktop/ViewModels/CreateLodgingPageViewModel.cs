using System;
using System.Reactive;
using System.Reactive.Linq;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for CreateLodging Page
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ReactiveViewModelBase" />
    public class CreateLodgingPageViewModel : ReactiveViewModelBase
    {
        private readonly Trip _trip;

        private string _error = string.Empty;

        private readonly LodgingManager _lodgingManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateLodgingPageViewModel" /> class.
        /// </summary>
        /// <param name="trip">The trip.</param>
        /// <param name="lodgingManager">The lodging manager.</param>
        /// <param name="screen">The screen.</param>
        public CreateLodgingPageViewModel(Trip trip, LodgingManager lodgingManager, IScreen screen) : base(screen, Guid.NewGuid().ToString()[..5])
        {
            _trip = trip;
            _lodgingManager = lodgingManager;
            HostScreen = screen;
            CreateLodgingCommand = ReactiveCommand.CreateFromObservable(createLodging);
            CancelCreateLodgingCommand =
                ReactiveCommand.CreateFromObservable(() => HostScreen.Router.NavigateBack.Execute());
        }

        public CreateLodgingPageViewModel(Trip trip, IScreen screen) : this(trip, new LodgingManager(), screen) { }

        /// <summary>
        ///     The create lodging command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> CreateLodgingCommand { get; }

        /// <summary>
        ///     The cancel create lodging command.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelCreateLodgingCommand { get; }

        /// <summary>
        ///     The error message.
        /// </summary>
        public string ErrorMessage
        {
            get => _error;
            set => this.RaiseAndSetIfChanged(ref _error, value);
        }

        /// <summary>
        ///     The validation manager.
        /// </summary>
        public ValidationManager ValidationManager { get; set; } = new();

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

        private IObservable<IRoutableViewModel> createLodging()
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

            var startDate = StartDate.Value.Date + StartTime.Value;

            var endDate = EndDate is null || EndTime is null ? _trip.EndDate : EndDate.Value.Date + EndTime.Value;

            var validDatesResponse = ValidationManager.DetermineIfValidEventDates(_trip.TripId, startDate, endDate);

            if (!string.IsNullOrEmpty(validDatesResponse.ErrorMessage))
            {
                ErrorMessage = validDatesResponse.ErrorMessage;
                return Observable.Empty<IRoutableViewModel>();
            }

            var resultResponse = _lodgingManager.CreateLodging(_trip.TripId, Location, startDate, endDate, Notes);
            if (string.IsNullOrEmpty(resultResponse.ErrorMessage))
                return HostScreen.Router.Navigate.Execute(new TripOverviewPageViewModel(_trip, HostScreen));

            ErrorMessage = resultResponse.ErrorMessage;
            return Observable.Empty<IRoutableViewModel>();
        }
    }
}