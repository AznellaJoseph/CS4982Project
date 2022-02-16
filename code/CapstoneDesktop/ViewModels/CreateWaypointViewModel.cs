using System;
using System.Diagnostics;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the CreateWaypoint Window
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class CreateWaypointViewModel : ViewModelBase
    {
        private readonly WaypointManager _waypointManager;

        private string _error = string.Empty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateWaypointViewModel" /> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public CreateWaypointViewModel(WaypointManager manager)
        {
            _waypointManager = manager;
            CreateWaypointCommand = ReactiveCommand.Create(createWaypoint);
            CancelCreateWaypointCommand = ReactiveCommand.Create(cancelCreateWaypoint);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateWaypointViewModel" /> class.
        /// </summary>
        public CreateWaypointViewModel() : this(new WaypointManager())
        {
        }

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
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The start time.
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// The end date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The end time.
        /// </summary>
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// The location.
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// The notes.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// The create waypoint command.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CreateWaypointCommand { get; set; }

        /// <summary>
        /// The cancel create waypoint command.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelCreateWaypointCommand { get; set; }

        private void createWaypoint()
        {
            if (string.IsNullOrEmpty(Location))
            {
                ErrorMessage = "You must enter a location for the waypoint";
            }
            else
            {
                var resultResponse = _waypointManager.CreateWaypoint(0, Location, StartDate + StartTime,
                    EndDate + EndTime, Notes);
                if (!string.IsNullOrEmpty(resultResponse.ErrorMessage))
                    ErrorMessage = resultResponse.ErrorMessage;
                else
                    Debug.WriteLine("Successfully created waypoint");
            }
        }

        private void cancelCreateWaypoint()
        {
        }
    }
}