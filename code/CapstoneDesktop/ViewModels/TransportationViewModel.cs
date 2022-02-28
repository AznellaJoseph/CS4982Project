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
    public class TransportationViewModel : ViewModelBase, IEventViewModel
    {
        private readonly IScreen _screen;
        
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

        public event EventHandler<EventArgs> RemoveEvent;
        
        /// <summary>
        ///     The Waypoint
        /// </summary>
        public Transportation Transportation { get; }

        /// <summary>
        ///     The Event.
        /// </summary>
        public IEvent Event => Transportation;
        
        public TransportationManager? FakeTransportationManager { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransportationViewModel" /> class.
        /// </summary>
        /// <param name="transportation">The transportation.</param>
        /// <param name="screen">The screen.</param>
        public TransportationViewModel(Transportation transportation, IScreen screen)
        {
            _screen = screen;
            Transportation = transportation;
            RemoveCommand = ReactiveCommand.Create(removeWaypoint);
        }

        private void removeWaypoint()
        {
            var manager = FakeTransportationManager ?? new TransportationManager();
            if (manager.RemoveTransportation(Transportation.TransportationId).StatusCode.Equals(200U))
            { 
                RemoveEvent?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Console.WriteLine("FAILED REMOVE");
            }
        }
    }
}