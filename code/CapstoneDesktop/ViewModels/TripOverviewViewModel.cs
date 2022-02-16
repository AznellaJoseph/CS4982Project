using Avalonia;
using Avalonia.Media;
using CapstoneBackend.Model;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Text;

namespace CapstoneDesktop.ViewModels
{
    public class TripOverviewViewModel : ViewModelBase
    {
        private string _tripName = String.Empty;
        private string _tripNotes = String.Empty;
        private DateTime _tripStartDate = DateTime.MinValue;
        private DateTime _tripEndDate = DateTime.MaxValue;

        /// <summary>Initializes a new instance of the <see cref="TripOverviewViewModel" /> class.</summary>
        /// <param name="selectedTrip">The selected trip to populate the view with info for.</param>
        public TripOverviewViewModel(Trip selectedTrip)
        {
            this._tripName = selectedTrip.Name;
            this._tripNotes = selectedTrip.Notes;
            this._tripStartDate = selectedTrip.StartDate;
            this._tripEndDate = selectedTrip.EndDate;

            LogoutCommand = ReactiveCommand.Create(this.Logout);
            ProfileCommand = ReactiveCommand.Create(this.OpenProfile);
            BackCommand = ReactiveCommand.Create(this.GoBackToTrips);
        }

        /// <summary>Initializes a new instance of the <see cref="TripOverviewViewModel" /> class.</summary>
        public TripOverviewViewModel()
        {
            LogoutCommand = ReactiveCommand.Create(this.Logout);
            ProfileCommand = ReactiveCommand.Create(this.OpenProfile);
            BackCommand = ReactiveCommand.Create(this.GoBackToTrips);
        }

        /// <summary>Gets or sets the name of the trip.</summary>
        /// <value>The name of the trip.</value>
        public string TripName 
        {
            get => _tripName.ToUpper();
            set => this.RaiseAndSetIfChanged(ref _tripName, value);
        }

        /// <summary>Gets or sets the trip notes.</summary>
        /// <value>The trip notes.</value>
        public string TripNotes
        {
            get => _tripNotes;
            set => this.RaiseAndSetIfChanged(ref _tripNotes, value);
        }

        /// <summary>Gets or sets the trip start date.</summary>
        /// <value>The trip start date.</value>
        public DateTime TripStartDate
        {
            get => _tripStartDate;
            set => this.RaiseAndSetIfChanged(ref _tripStartDate, value);
        }

        /// <summary>Gets or sets the trip end date.</summary>
        /// <value>The trip end date.</value>
        public DateTime TripEndDate
        {
            get => _tripEndDate;
            set => this.RaiseAndSetIfChanged(ref _tripEndDate, value);
        }

        /// <summary>Gets or sets the logout command.</summary>
        /// <value>The logout command.</value>
        public ReactiveCommand<Unit, Unit> LogoutCommand { get; set; }

        /// <summary>Gets or sets the profile command.</summary>
        /// <value>The profile command.</value>
        public ReactiveCommand<Unit, Unit> ProfileCommand { get; set; }

        /// <summary>Gets or sets the back command.</summary>
        /// <value>The back command.</value>
        public ReactiveCommand<Unit, Unit> BackCommand { get; set; }

        private void Logout()
        {
            Debug.WriteLine("Log User Out");
        }

        private void OpenProfile()
        {
            Debug.WriteLine("Open Profile");
        }

        private void GoBackToTrips()
        {
            Debug.WriteLine("Go back to trips");
        }

    }
}
