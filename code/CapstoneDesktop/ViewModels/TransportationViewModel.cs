using System;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    /// ViewModel for a single Waypoint
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ReactiveViewModelBase" />
    /// <seealso cref="CapstoneDesktop.ViewModels.IEventViewModel" />
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class TransportationViewModel : ReactiveViewModelBase, IEventViewModel
    {
        private readonly IScreen _screen;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransportationViewModel" /> class.
        /// </summary>
        /// <param name="transportation">The transportation.</param>
        /// <param name="screen">The screen.</param>
        public TransportationViewModel(Transportation transportation, IScreen screen) : base(screen,
            Guid.NewGuid().ToString()[..5])
        {
            _screen = screen;
            Transportation = transportation;
            RemoveCommand = ReactiveCommand.Create(removeWaypoint);
        }

        /// <summary>
        ///     The transportation
        /// </summary>
        public Transportation Transportation { get; }

        /// <summary>
        ///     The fake transportation manager
        /// </summary>
        public TransportationManager? FakeTransportationManager { get; set; }

        /// <summary>
        ///     The remove command
        /// </summary>
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

        /// <summary>
        ///     The remove event
        /// </summary>
        public event EventHandler<EventArgs>? RemoveEvent;

        /// <summary>
        ///     The Event.
        /// </summary>
        public IEvent Event => Transportation;

        private void removeWaypoint()
        {
            var manager = FakeTransportationManager ?? new TransportationManager();
            if (manager.RemoveTransportation(Transportation.TransportationId).Data)
                RemoveEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}