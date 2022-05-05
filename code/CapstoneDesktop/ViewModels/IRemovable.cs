using System;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     IRemovable Interface to aid in removing IEvent Types (Waypoint, Transportation)
    /// </summary>
    public interface IRemovable
    {
        /// <summary>
        ///     The remove event.
        /// </summary>
        public event EventHandler<EventArgs> RemoveEvent;
    }
}