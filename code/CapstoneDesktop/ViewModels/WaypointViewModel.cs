using System;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    public class WaypointViewModel : ViewModelBase
    {

        private readonly IScreen _screen;
        // public Waypoint Waypoint { get; }
        
        public ReactiveCommand<Unit, Unit> TripClickCommand { get; }

        public WaypointViewModel(IScreen screen)
        {
            this._screen = screen;
            // this.Trip = trip;
            this.TripClickCommand = ReactiveCommand.Create(this.clickTrip);
        }

        private void clickTrip()
        {
            //TODO redirect to Trip View
        }
        
    }
}