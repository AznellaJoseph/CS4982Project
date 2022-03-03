using System;
using System.Reactive;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     IRemovable Interface to aid in removing IEvent Types (Waypoint, Transportation)
    /// </summary>
    public interface IRemovable
    {
        /// <summary>
        ///     The remove command.
        /// </summary>
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

        /// <summary>
        ///     The remove event.
        /// </summary>
        public event EventHandler<EventArgs> RemoveEvent;
    }
}