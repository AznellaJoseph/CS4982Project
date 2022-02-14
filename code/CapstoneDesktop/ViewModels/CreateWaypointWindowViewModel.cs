﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    public class CreateWaypointWindowViewModel : ViewModelBase
    {
        private readonly WaypointManager _waypointManager;

        public CreateWaypointWindowViewModel(WaypointManager manager)
        {
            this._waypointManager = manager;
            this.CreateWaypointCommand = ReactiveCommand.Create(this.createWaypoint);
            this.CancelCreateWaypointCommand = ReactiveCommand.Create(this.cancelCreateWaypoint);
        }

        public CreateWaypointWindowViewModel() : this(new WaypointManager())
        {
        }

        private string _error = string.Empty;

        public string ErrorMessage
        {
            get => this._error;
            set => this.RaiseAndSetIfChanged(ref this._error, value);
        }

        public DateTime StartDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan EndTime { get; set; }

        public string? WaypointLocation { get; set; }
        public string? Notes { get; set; }

        public ReactiveCommand<Unit, Unit> CreateWaypointCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CancelCreateWaypointCommand { get; set; }

        private void createWaypoint()
        {
            if (string.IsNullOrEmpty(WaypointLocation))
            {
                ErrorMessage = "You must enter a location for the waypoint";
            }
            else
            {
                var resultResponse = _waypointManager.CreateWaypoint(0, WaypointLocation, StartDate + StartTime,
                    EndDate + EndTime, Notes);
                if (!string.IsNullOrEmpty(resultResponse.ErrorMessage))
                {
                    ErrorMessage = resultResponse.ErrorMessage;
                }
                else
                {
                    Debug.WriteLine("Successfully created waypoint");
                }
            }
        }

        private void cancelCreateWaypoint()
        {

        }
    }
}