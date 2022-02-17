using System;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    public class WaypointViewModel : ViewModelBase
    {

        private readonly IScreen _screen;
        public Waypoint Waypoint { get; }

        public WaypointViewModel(Waypoint waypoint, IScreen screen)
        {
            this._screen = screen;
            this.Waypoint = waypoint;
        }

    }
}