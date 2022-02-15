using System;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    public class TripViewModel : ViewModelBase
    {
        public Trip Trip { get; }
        
       

        public ReactiveCommand<Unit, Unit> TripClickCommand { get; }

        public TripViewModel(Trip trip)
        {
            this.Trip = trip;
            this.TripClickCommand = ReactiveCommand.Create(this.clickTrip);
        }

        private void clickTrip()
        {
        }
        
    }
}