using CapstoneBackend.Model;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Data.Converters;
using CapstoneDesktop.Converters;

namespace CapstoneDesktop.ViewModels
{
    public class TripOverviewPageViewModel : ViewModelBase, IRoutableViewModel
    {
        
        private readonly WaypointManager _waypointManager;
        private DateTime? _selectedDate;

        public IScreen HostScreen { get; }
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ReactiveCommand<Unit, IRoutableViewModel> LogoutCommand { get; }
        
        public ReactiveCommand<Unit, Unit> BackCommand { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> CreateWaypointCommand { get; }
        public ObservableCollection<WaypointViewModel> WaypointViewModels { get; }
        
        public Trip Trip { get; }

        public DateTime? SelectedDate
        {
            get => this._selectedDate;
            set
            {
                this.RaiseAndSetIfChanged(ref this._selectedDate, value, nameof(SelectedDate));
                this.updateWaypoints();
            }
        }

        public TripOverviewPageViewModel(Trip trip, WaypointManager waypointManager, IScreen screen)
        {
            this.Trip = trip;
            this._waypointManager = waypointManager;
            this.HostScreen = screen;
            this.LogoutCommand = ReactiveCommand.CreateFromObservable(() => this.HostScreen.Router.Navigate.Execute(new LoginPageViewModel(this.HostScreen)));
            this.CreateWaypointCommand = ReactiveCommand.CreateFromObservable(() => this.HostScreen.Router.Navigate.Execute(new CreateWaypointPageViewModel(this.Trip, this.HostScreen)));
            this.BackCommand = ReactiveCommand.CreateFromObservable(() => this.HostScreen.Router.NavigateBack.Execute());
            this.WaypointViewModels = new ObservableCollection<WaypointViewModel>();
        }

        private void updateWaypoints()
        {
            this.WaypointViewModels.Clear();
            if (this.SelectedDate is not null)
            {
                var response = this._waypointManager.GetWaypointsOnDate(this.Trip.TripId, (DateTime)this.SelectedDate);
                foreach (var waypoint in response.Data ?? new List<Waypoint>())
                {
                    this.WaypointViewModels.Add(new WaypointViewModel(waypoint, this.HostScreen));
                }
            }
        }

        public TripOverviewPageViewModel(Trip trip, IScreen screen) : this(trip, new WaypointManager(), screen)
        {
        }

    }
}
