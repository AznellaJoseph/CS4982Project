using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for a single Waypoint
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class WaypointViewModel : ViewModelBase
    {
        private readonly IScreen _screen;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WaypointViewModel" /> class.
        /// </summary>
        /// <param name="waypoint">The waypoint.</param>
        /// <param name="screen">The screen.</param>
        public WaypointViewModel(Waypoint waypoint, IScreen screen)
        {
            _screen = screen;
            Waypoint = waypoint;
        }

        /// <summary>
        ///     The waypoint.
        /// </summary>
        public Waypoint Waypoint { get; }
    }
}