using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for a single Trip
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class TripViewModel : ViewModelBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TripViewModel" /> class.
        /// </summary>
        /// <param name="trip">The trip.</param>
        /// <param name="screen">The screen.</param>
        public TripViewModel(Trip trip, IScreen screen)
        {
            Trip = trip;
            TripClickCommand = ReactiveCommand.CreateFromObservable(() =>
                screen.Router.Navigate.Execute(new TripOverviewPageViewModel(Trip, screen, new LodgingManager())));
        }

        /// <summary>
        ///     The trip.
        /// </summary>
        public Trip Trip { get; }

        /// <summary>
        ///     The trip click command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> TripClickCommand { get; }
    }
}