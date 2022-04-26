﻿using System;
using System.Reactive;
using System.Reactive.Linq;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    public class EditWaypointPageViewModel : ReactiveViewModelBase
    {
        private string _error = string.Empty;
        private readonly Waypoint _waypoint;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EditWaypointPageViewModel" /> class.
        /// </summary>
        /// <param name="waypoint">The waypoint.</param>
        /// <param name="screen">The screen.</param>
        public EditWaypointPageViewModel(Waypoint waypoint, IScreen screen) : base(screen,
            Guid.NewGuid().ToString()[..5])
        {
            _waypoint = waypoint;
            HostScreen = screen;
            StartDate = _waypoint.StartDate.Date;
            StartTime = _waypoint.StartDate.TimeOfDay;
            EndDate = _waypoint.EndDate.Date;
            EndTime = _waypoint.EndDate.TimeOfDay;
            Location = _waypoint.Location;
            Notes = _waypoint.Notes;
            EditWaypointCommand = ReactiveCommand.CreateFromObservable(editWaypoint);
            CancelEditWaypointCommand =
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
        ///     The edit waypoint command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> EditWaypointCommand { get; }

        /// <summary>
        ///     The cancel edit waypoint command.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelEditWaypointCommand { get; }

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
        public string? Location { get; set; }

        /// <summary>
        ///     The notes.
        /// </summary>
        public string? Notes { get; set; }

        private IObservable<IRoutableViewModel> editWaypoint()
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

            var endDate = EndDate is null || EndTime is null
                ? _waypoint.EndDate
                : EndDate.Value.Date + EndTime.Value;

            var validDatesResponse =
                ValidationManager.DetermineIfValidEventDates(_waypoint.TripId, startDate, endDate);

            if (!string.IsNullOrEmpty(validDatesResponse.ErrorMessage))
            {
                ErrorMessage = validDatesResponse.ErrorMessage;
                return Observable.Empty<IRoutableViewModel>();
            }

            var updatedWaypoint = new Waypoint
            {
                WaypointId = _waypoint.WaypointId,
                TripId = _waypoint.TripId,
                EndDate = endDate,
                Location = Location,
                Notes = Notes ?? string.Empty,
                StartDate = startDate
            };

            var updatedWaypointResponse = WaypointManager.EditWaypoint(updatedWaypoint);

            if (string.IsNullOrEmpty(updatedWaypointResponse.ErrorMessage))
                return HostScreen.Router.Navigate.Execute(new TripOverviewPageViewModel(
                    new TripManager().GetTripByTripId(_waypoint.TripId).Data, HostScreen, new LodgingManager()));
            ErrorMessage = updatedWaypointResponse.ErrorMessage;
            return Observable.Empty<IRoutableViewModel>();
        }
    }
}