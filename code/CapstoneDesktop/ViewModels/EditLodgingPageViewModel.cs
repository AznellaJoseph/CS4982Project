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
    ///     ViewModel for the Edit Lodging Page
    /// </summary>
    public class EditLodgingPageViewModel : ReactiveViewModelBase
    {
        private readonly Lodging _lodging;
        private string _error = string.Empty;
        private string _location = string.Empty;

        private IEnumerable<string> _predictions = new List<string>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="EditLodgingPageViewModel" /> class.
        /// </summary>
        /// <param name="lodging">The lodging being edited.</param>
        /// <param name="screen">The screen.</param>
        public EditLodgingPageViewModel(Lodging lodging, IScreen screen) : base(screen,
            Guid.NewGuid().ToString()[..5])
        {
            _lodging = lodging;
            HostScreen = screen;
            StartDate = _lodging.StartDate.Date;
            StartTime = _lodging.StartDate.TimeOfDay;
            EndDate = _lodging.EndDate.Date;
            EndTime = _lodging.EndDate.TimeOfDay;
            Location = _lodging.Location;
            Notes = _lodging.Notes;
            EditLodgingCommand = ReactiveCommand.CreateFromObservable(editLodging);
            CancelEditLodgingCommand =
                ReactiveCommand.CreateFromObservable(() => HostScreen.Router.NavigateBack.Execute());
        }

        /// <summary>
        ///     The lodging manager.
        /// </summary>
        public LodgingManager LodgingManager { get; set; } = new();

        /// <summary>
        ///     The validation manager.
        /// </summary>
        public ValidationManager ValidationManager { get; set; } = new();

        /// <summary>
        ///     The edit lodging command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> EditLodgingCommand { get; }

        /// <summary>
        ///     The cancel edit lodging command.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelEditLodgingCommand { get; }

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
        ///     The notes.
        /// </summary>
        public string? Notes { get; set; }

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

        private async void UpdateAutoCompleteResultsAsync()
        {
            AutocompletePredictions = await GooglePlacesService.Autocomplete(Location);
        }

        private IObservable<IRoutableViewModel> editLodging()
        {
            if (string.IsNullOrEmpty(Location))
            {
                ErrorMessage = Ui.ErrorMessages.EmptyLocation;
                return Observable.Empty<IRoutableViewModel>();
            }

            var validLocationResponse = ValidationManager.DetermineIfValidLocation(Location);

            if (!string.IsNullOrEmpty(validLocationResponse.ErrorMessage))
            {
                ErrorMessage = validLocationResponse.ErrorMessage;
                return Observable.Empty<IRoutableViewModel>();
            }

            if (StartDate is null || StartTime is null)
            {
                ErrorMessage = Ui.ErrorMessages.InvalidEventDate;
                return Observable.Empty<IRoutableViewModel>();
            }

            var startDate = StartDate.Value.Date + StartTime.Value;

            var endDate = EndDate is null || EndTime is null
                ? _lodging.EndDate
                : EndDate.Value.Date + EndTime.Value;

            var validDatesResponse =
                ValidationManager.DetermineIfValidEventDates(_lodging.TripId, startDate, endDate);

            if (!string.IsNullOrEmpty(validDatesResponse.ErrorMessage))
            {
                ErrorMessage = validDatesResponse.ErrorMessage;
                return Observable.Empty<IRoutableViewModel>();
            }

            var updatedLodging = new Lodging
            {
                LodgingId = _lodging.LodgingId,
                TripId = _lodging.TripId,
                EndDate = endDate,
                Location = Location,
                Notes = Notes ?? string.Empty,
                StartDate = startDate
            };

            var updatedLodgingResponse = LodgingManager.EditLodging(updatedLodging);

            if (string.IsNullOrEmpty(updatedLodgingResponse.ErrorMessage))
                return HostScreen.Router.Navigate.Execute(new TripOverviewPageViewModel(
                    new TripManager().GetTripByTripId(_lodging.TripId).Data, HostScreen, new LodgingManager()));
            ErrorMessage = updatedLodgingResponse.ErrorMessage;
            return Observable.Empty<IRoutableViewModel>();
        }
    }
}