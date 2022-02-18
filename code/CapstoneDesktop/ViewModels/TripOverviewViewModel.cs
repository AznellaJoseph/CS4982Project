using System;
using System.Diagnostics;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the Trip Overview
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class TripOverviewViewModel : ViewModelBase
    {
        private DateTime _tripEndDate = DateTime.MaxValue;
        private string _tripName = string.Empty;
        private string _tripNotes = string.Empty;
        private DateTime _tripStartDate = DateTime.MinValue;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TripOverviewViewModel" /> class.
        /// </summary>
        /// <param name="selectedTrip">The selected trip to populate the view with info for.</param>
        public TripOverviewViewModel(Trip selectedTrip)
        {
            _tripName = selectedTrip.Name;
            _tripNotes = selectedTrip.Notes;
            _tripStartDate = selectedTrip.StartDate;
            _tripEndDate = selectedTrip.EndDate;

            LogoutCommand = ReactiveCommand.Create(logout);
            ProfileCommand = ReactiveCommand.Create(openProfile);
            BackCommand = ReactiveCommand.Create(goBackToTrips);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TripOverviewViewModel" /> class.
        /// </summary>
        public TripOverviewViewModel()
        {
            LogoutCommand = ReactiveCommand.Create(logout);
            ProfileCommand = ReactiveCommand.Create(openProfile);
            BackCommand = ReactiveCommand.Create(goBackToTrips);
        }

        /// <summary>
        ///     The trip name
        /// </summary>
        public string TripName
        {
            get => _tripName.ToUpper();
            set => this.RaiseAndSetIfChanged(ref _tripName, value);
        }

        /// <summary>
        ///     The trip notes
        /// </summary>
        public string TripNotes
        {
            get => _tripNotes;
            set => this.RaiseAndSetIfChanged(ref _tripNotes, value);
        }

        /// <summary>
        ///     The trip start date
        /// </summary>
        public DateTime TripStartDate
        {
            get => _tripStartDate;
            set => this.RaiseAndSetIfChanged(ref _tripStartDate, value);
        }

        /// <summary>
        ///     The trip end date
        /// </summary>
        public DateTime TripEndDate
        {
            get => _tripEndDate;
            set => this.RaiseAndSetIfChanged(ref _tripEndDate, value);
        }

        /// <summary>
        ///     The login command
        /// </summary>
        public ReactiveCommand<Unit, Unit> LogoutCommand { get; set; }

        /// <summary>
        ///     The profile command
        /// </summary>
        public ReactiveCommand<Unit, Unit> ProfileCommand { get; set; }

        /// <summary>
        ///     The back command
        /// </summary>
        public ReactiveCommand<Unit, Unit> BackCommand { get; set; }

        private void logout()
        {
            Debug.WriteLine("Log User Out");
        }

        private void openProfile()
        {
            Debug.WriteLine("Open Profile");
        }

        private void goBackToTrips()
        {
            Debug.WriteLine("Go back to trips");
        }
    }
}