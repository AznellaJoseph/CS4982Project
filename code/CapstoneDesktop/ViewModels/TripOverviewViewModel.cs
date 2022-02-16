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
        private string _tripName = "Test Trip";
        private string _tripNotes = "Some Notes";
        private DateTime _tripStartDate = new DateTime(2022, 1, 8);
        private DateTime _tripEndDate = new DateTime(2022, 1, 20);


        public TripOverviewViewModel(Trip selectedTrip)
        {
            this.TripName = selectedTrip.Name;
            this.TripNotes = selectedTrip.Notes;
            this.TripStartDate = selectedTrip.StartDate;
            this.TripEndDate = selectedTrip.EndDate;

            LogoutCommand = ReactiveCommand.Create(this.Logout);
            ProfileCommand = ReactiveCommand.Create(this.OpenProfile);
            BackCommand = ReactiveCommand.Create(this.GoBackToTrips);
        }

        public TripOverviewViewModel()
        {
            LogoutCommand = ReactiveCommand.Create(this.Logout);
            ProfileCommand = ReactiveCommand.Create(this.OpenProfile);
            BackCommand = ReactiveCommand.Create(this.GoBackToTrips);
        }

        public string TripName 
        {
            get => _tripName.ToUpper();
            set => this.RaiseAndSetIfChanged(ref _tripName, value);
        }

        public string TripNotes
        {
            get => _tripNotes;
            set => this.RaiseAndSetIfChanged(ref _tripNotes, value);
        }

        public DateTime TripStartDate
        {
            get => _tripStartDate;
            set => this.RaiseAndSetIfChanged(ref _tripStartDate, value);
        }

        public DateTime TripEndDate
        {
            get => _tripEndDate;
            set => this.RaiseAndSetIfChanged(ref _tripEndDate, value);
        }

        public ReactiveCommand<Unit, Unit> LogoutCommand { get; set; }

        public ReactiveCommand<Unit, Unit> ProfileCommand { get; set; }

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
