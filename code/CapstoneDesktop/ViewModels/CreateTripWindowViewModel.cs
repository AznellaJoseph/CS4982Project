using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{

    /// <summary>
    /// ViewModel for Create Trip Window
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class CreateTripWindowViewModel : ViewModelBase
    {
        private readonly TripManager _tripManager;

        private string _error = string.Empty;

        public string ErrorMessage
        {
            get => this._error;
            set => this.RaiseAndSetIfChanged(ref _error, value);
        }

        public string? TripName { get; set; }
        public string? Notes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ReactiveCommand<Unit, Unit> CreateTripCommand { get; set; }
        public ReactiveCommand<Unit, Unit> CancelCreateTripCommand { get; set; }

        public CreateTripWindowViewModel(TripManager manager)
        {
            this._tripManager = manager;
            this.CreateTripCommand = ReactiveCommand.Create(this.createTrip);
            this.CancelCreateTripCommand = ReactiveCommand.Create(this.cancelCreateTrip);
        }

        public CreateTripWindowViewModel() : this(new TripManager())
        {

        }

        private void createTrip()
        {

        }

        private void cancelCreateTrip()
        {

        }

    }
}
