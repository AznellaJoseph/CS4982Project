using System;
using System.Reactive;
using System.Reactive.Linq;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the Edit Transportation Page
    /// </summary>
    public class EditTransportationPageViewModel : ReactiveViewModelBase
    {
        private readonly Transportation _transportation;
        private string _error = string.Empty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EditTransportationPageViewModel" /> class.
        /// </summary>
        /// <param name="transportation">The transportation being edited.</param>
        /// <param name="screen">The screen.</param>
        public EditTransportationPageViewModel(Transportation transportation, IScreen screen) : base(screen,
            Guid.NewGuid().ToString()[..5])
        {
            _transportation = transportation;
            HostScreen = screen;
            StartDate = _transportation.StartDate.Date;
            StartTime = _transportation.StartDate.TimeOfDay;
            EndDate = _transportation.EndDate.Date;
            EndTime = _transportation.EndDate.TimeOfDay;
            Method = _transportation.Method;
            Notes = _transportation.Notes;
            EditTransportationCommand = ReactiveCommand.CreateFromObservable(editTransportation);
            CancelEditTransportationCommand =
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
        ///     The edit transportation command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> EditTransportationCommand { get; }

        /// <summary>
        ///     The cancel edit transportation command.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelEditTransportationCommand { get; }

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

        private IObservable<IRoutableViewModel> editTransportation()
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

            var endDate = EndDate is null || EndTime is null
                ? _transportation.EndDate
                : EndDate.Value.Date + EndTime.Value;

            var validDatesResponse =
                ValidationManager.DetermineIfValidEventDates(_transportation.TripId, startDate, endDate);

            if (!string.IsNullOrEmpty(validDatesResponse.ErrorMessage))
            {
                ErrorMessage = validDatesResponse.ErrorMessage;
                return Observable.Empty<IRoutableViewModel>();
            }

            if (startDate.CompareTo(endDate) > 0)
            {
                ErrorMessage = Ui.ErrorMessages.InvalidStartDate;
                return Observable.Empty<IRoutableViewModel>();
            }

            var updatedTransportation = new Transportation
            {
                TransportationId = _transportation.TransportationId,
                TripId = _transportation.TripId,
                EndDate = endDate,
                Method = Method,
                Notes = Notes ?? string.Empty,
                StartDate = startDate
            };

            var clashingEventResponse =
                ValidationManager.FindClashingEvent(_transportation.TripId, startDate, endDate);

            if (!string.IsNullOrEmpty(clashingEventResponse.ErrorMessage))
            {
                var clashingEvent = clashingEventResponse.Data;
                if (clashingEvent is null || !clashingEvent.Equals(_transportation))
                {
                    ErrorMessage = clashingEventResponse.ErrorMessage;
                    return Observable.Empty<IRoutableViewModel>();
                }
            }

            var updatedTransportationResponse = TransportationManager.EditTransportation(updatedTransportation);

            if (string.IsNullOrEmpty(updatedTransportationResponse.ErrorMessage))
                return HostScreen.Router.Navigate.Execute(new TripOverviewPageViewModel(
                    new TripManager().GetTripByTripId(_transportation.TripId).Data, HostScreen, new LodgingManager()));
            ErrorMessage = updatedTransportationResponse.ErrorMessage;
            return Observable.Empty<IRoutableViewModel>();
        }
    }
}