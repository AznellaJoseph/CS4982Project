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
    public class CreateTransportationPageViewModel : ReactiveViewModelBase
    {
        private readonly Trip _trip;
        private readonly TransportationManager _transportationManager;

        private string _error = string.Empty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateTransportationPageViewModel" /> class.
        /// </summary>
        /// <param name="trip">the trip that the transportation is for.</param>
        /// <param name="manager">The transportation manager.</param>
        /// <param name="screen">The host screen</param>
        public CreateTransportationPageViewModel(Trip trip, TransportationManager manager, IScreen screen) : base(screen,
            Guid.NewGuid().ToString()[..5])
        {
            _trip = trip;
            _transportationManager = manager;
            HostScreen = screen;
            CreateTransportationCommand = ReactiveCommand.CreateFromObservable(createTransportation);
            CancelCreateTransportationCommand =
                ReactiveCommand.CreateFromObservable(() => HostScreen.Router.NavigateBack.Execute());
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateWaypointPageViewModel" /> class.
        /// </summary>
        /// <param name="trip">The trip.</param>
        /// <param name="screen">The screen.</param>
        public CreateTransportationPageViewModel(Trip trip, IScreen screen) : this(trip, new TransportationManager(), screen)
        {
        }

        /// <summary>
        ///     The create transportation command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> CreateTransportationCommand { get; }

        /// <summary>
        ///     The cancel create transportation command.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelCreateTransportationCommand { get; }

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
        public string? Method { get; set; }

        /// <summary>
        ///     The notes.
        /// </summary>
        public string? Notes { get; set; }

        private IObservable<IRoutableViewModel> createTransportation()
        {
            if (string.IsNullOrEmpty(Method))
            {
                ErrorMessage = Ui.ErrorMessages.EmptyWaypointLocation;
                return Observable.Empty<IRoutableViewModel>();
            }

            if (StartDate is null || StartTime is null)
            {
                ErrorMessage = Ui.ErrorMessages.NullWaypointStartDate;
                return Observable.Empty<IRoutableViewModel>();
            }

            var startDate = StartDate.Value.Date + StartTime.Value;

            var endTime = EndDate is null || EndTime is null ? _trip.EndDate : EndDate.Value.Date + EndTime.Value;

            if (startDate.CompareTo(_trip.StartDate) < 0 || startDate.CompareTo(_trip.EndDate) > 0 ||
                endTime.CompareTo(_trip.StartDate) < 0 || endTime.CompareTo(_trip.EndDate) > 0)
            {
                ErrorMessage = Ui.ErrorMessages.InvalidWaypointDate;
                return Observable.Empty<IRoutableViewModel>();
            }

            var resultResponse = _transportationManager.CreateTransportation(_trip.TripId, Method, startDate, endTime);
            if (string.IsNullOrEmpty(resultResponse.ErrorMessage))
                return HostScreen.Router.Navigate.Execute(new TripOverviewPageViewModel(_trip, HostScreen));

            ErrorMessage = resultResponse.ErrorMessage;
            return Observable.Empty<IRoutableViewModel>();
        }
    }
}