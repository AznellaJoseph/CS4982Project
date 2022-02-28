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
    public class WaypointViewModel : ViewModelBase, IEventViewModel
    {
        private readonly IScreen _screen;
        
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

        public event EventHandler<EventArgs> RemoveEvent;
        
        /// <summary>
        ///     The Waypoint
        /// </summary>
        public Waypoint Waypoint { get; }

        /// <summary>
        ///     The Event.
        /// </summary>
        public IEvent Event => Waypoint;
        
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
            if (manager.RemoveWaypoint(Waypoint.WaypointId).Data)
            {
                RemoveEvent?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}