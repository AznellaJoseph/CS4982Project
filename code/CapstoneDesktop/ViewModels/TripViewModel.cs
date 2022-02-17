using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the Trip
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class TripViewModel : ViewModelBase
    {
        private readonly IScreen _screen;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TripViewModel" /> class.
        /// </summary>
        /// <param name="trip">The trip.</param>
        /// <param name="screen">The screen.</param>
        public TripViewModel(Trip trip, IScreen screen)
        {
            _screen = screen;
            Trip = trip;
            TripClickCommand = ReactiveCommand.Create(clickTrip);
        }

        /// <summary>
        ///     The trip
        /// </summary>
        public Trip Trip { get; }

        /// <summary>
        ///     The trip click command
        /// </summary>
        public ReactiveCommand<Unit, Unit> TripClickCommand { get; }

        private void clickTrip()
        {
            //TODO redirect to Trip View
        }
    }
}