using System;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for a single Waypoint
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ReactiveViewModelBase" />
    /// <seealso cref="CapstoneDesktop.ViewModels.IEventViewModel" />
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class WaypointViewModel : ReactiveViewModelBase, IEventViewModel
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WaypointViewModel" /> class.
        /// </summary>
        /// <param name="waypoint">The waypoint.</param>
        /// <param name="screen">The screen.</param>
        public WaypointViewModel(Waypoint waypoint, IScreen screen) : base(screen, Guid.NewGuid().ToString()[..5])
        {
            HostScreen = screen;
            Waypoint = waypoint;
            RemoveCommand = ReactiveCommand.Create(removeWaypoint);
            ViewCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new EventPageViewModel(waypoint, screen)));
        }

        /// <summary>
        ///     The Waypoint
        /// </summary>
        public Waypoint Waypoint { get; }

        /// <summary>
        ///     The waypoint manager
        /// </summary>
        public WaypointManager WaypointManager { get; set; } = new();

        /// <summary>
        ///     The remove command
        /// </summary>
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

        /// <summary>
        ///     The remove event
        /// </summary>
        public event EventHandler<EventArgs>? RemoveEvent;

        /// <summary>
        ///     The view command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> ViewCommand { get; }

        /// <summary>
        ///     The Event.
        /// </summary>
        public IEvent Event => Waypoint;

        private void removeWaypoint()
        {
            if (WaypointManager.RemoveWaypoint(Waypoint.WaypointId).Data) RemoveEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}