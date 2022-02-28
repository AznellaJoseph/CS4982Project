using System;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for a single Waypoint
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class WaypointViewModel : ViewModelBase, IRemovable
    {
        private readonly IScreen _screen;
        
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

        public event EventHandler<EventArgs> RemoveEvent;
        
        /// <summary>
        ///     The waypoint.
        /// </summary>
        public Waypoint Waypoint { get; }
        
        public WaypointManager? FakeWaypointManager { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WaypointViewModel" /> class.
        /// </summary>
        /// <param name="waypoint">The waypoint.</param>
        /// <param name="screen">The screen.</param>
        public WaypointViewModel(Waypoint waypoint, IScreen screen)
        {
            _screen = screen;
            Waypoint = waypoint;
            RemoveCommand = ReactiveCommand.Create(removeWaypoint);
        }

        private void removeWaypoint()
        {
            var manager = FakeWaypointManager ?? new WaypointManager();
            if (manager.RemoveWaypoint(Waypoint.WaypointId).StatusCode.Equals(200U))
            {
                RemoveEvent?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}