using System;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;
using Observable = System.Reactive.Linq.Observable;

namespace CapstoneDesktop.ViewModels
{
    public class TripViewModel : ViewModelBase
    {
        public Trip Trip { get; }
        
        public ReactiveCommand<Unit, IRoutableViewModel> TripClickCommand { get; }

        public TripViewModel(Trip trip, IScreen screen)
        {
            this.Trip = trip;
            this.TripClickCommand = ReactiveCommand.CreateFromObservable(() => screen.Router.Navigate.Execute(new TripOverviewPageViewModel(this.Trip, screen)));
        }

    }
}