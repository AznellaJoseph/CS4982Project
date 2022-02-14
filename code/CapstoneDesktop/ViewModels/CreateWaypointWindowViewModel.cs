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
    public class CreateWaypointWindowViewModel : ViewModelBase
    {
        private readonly WaypointManager _waypointManager;

        private string _error = string.Empty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateWaypointWindowViewModel" /> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public CreateWaypointWindowViewModel(WaypointManager manager)
        {
            _waypointManager = manager;
            CreateWaypointCommand = ReactiveCommand.Create(createWaypoint);
            CancelCreateWaypointCommand = ReactiveCommand.Create(cancelCreateWaypoint);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateWaypointWindowViewModel" /> class.
        /// </summary>
        public CreateWaypointWindowViewModel() : this(new WaypointManager())
        {
        }

        public string ErrorMessage
        {
            get => _error;
            set => this.RaiseAndSetIfChanged(ref _error, value);
        }

        public DateTime StartDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan EndTime { get; set; }

        public string? WaypointLocation { get; set; }
        public string? Notes { get; set; }

        public ReactiveCommand<Unit, Unit> CreateWaypointCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CancelCreateWaypointCommand { get; set; }

        private void createWaypoint()
        {
            if (string.IsNullOrEmpty(WaypointLocation))
            {
                ErrorMessage = "You must enter a location for the waypoint";
            }
            else
            {
                var resultResponse = _waypointManager.CreateWaypoint(0, WaypointLocation, StartDate + StartTime,
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