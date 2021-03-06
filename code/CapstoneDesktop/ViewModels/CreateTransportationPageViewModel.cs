using System;
using System.Reactive;
using System.Reactive.Linq;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the Create Transportation Page
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class CreateTransportationPageViewModel : ReactiveViewModelBase
    {
        private readonly Trip _trip;

        private string _error = string.Empty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateTransportationPageViewModel" /> class.
        /// </summary>
        /// <param name="trip">the trip the transportation is being created for.</param>
        /// <param name="screen">The screen</param>
        public CreateTransportationPageViewModel(Trip trip, IScreen screen) : base(
            screen,
            Guid.NewGuid().ToString()[..5])
        {
            _trip = trip;
            HostScreen = screen;
            CreateTransportationCommand = ReactiveCommand.CreateFromObservable(createTransportation);
            CancelCreateTransportationCommand =
                ReactiveCommand.CreateFromObservable(() => HostScreen.Router.NavigateBack.Execute());
        }

        /// <summary>
        ///     The transportation manager.
        /// </summary>
        public TransportationManager TransportationManager { get; set; } = new();

        /// <summary>
        ///     The validation manager.
        /// </summary>
        public ValidationManager ValidationManager { get; set; } = new();

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
        ///     The method.
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
                ErrorMessage = Ui.ErrorMessages.EmptyTransportationMethod;
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

            var clashingEventResponse =
                ValidationManager.FindClashingEvents(_trip.TripId, startDate, endDate, null);

            if (!string.IsNullOrEmpty(clashingEventResponse.ErrorMessage))
            {
                ErrorMessage = clashingEventResponse.ErrorMessage;
                return Observable.Empty<IRoutableViewModel>();
            }

            var resultResponse =
                TransportationManager.CreateTransportation(_trip.TripId, Method, startDate, endDate, Notes);
            if (string.IsNullOrEmpty(resultResponse.ErrorMessage))
                return HostScreen.Router.Navigate.Execute(new TripOverviewPageViewModel(_trip, HostScreen,
                    new LodgingManager()));

            ErrorMessage = resultResponse.ErrorMessage;
            return Observable.Empty<IRoutableViewModel>();
        }
    }
}