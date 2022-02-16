using System;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    public class TripViewModel : ViewModelBase
    {

        private readonly IScreen _screen;
        public Trip Trip { get; }
        
        public ReactiveCommand<Unit, Unit> TripClickCommand { get; }

        public TripViewModel(Trip trip, IScreen screen)
        {
            this._screen = screen;
            this.Trip = trip;
            this.TripClickCommand = ReactiveCommand.Create(this.clickTrip);
        }

        private void clickTrip()
        {
            //TODO redirect to Trip View
        }
        
    }
}